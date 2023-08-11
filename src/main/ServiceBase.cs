using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BoltPay.Authentication;

namespace BoltPay;

public abstract class ServiceBase
{
    private readonly HttpClient _client;
    private readonly string _baseUrl;
    private readonly AuthenticationBase _authentication;

    public ServiceBase(HttpClient client, string baseUrl, AuthenticationBase authentication)
    {
        _client = client;
        _baseUrl = baseUrl;
        _authentication = authentication;
    }
   
   /// <summary>
   /// 
   /// </summary>
   /// <param name="url"></param>
   /// <typeparam name="TResponse"></typeparam>
   /// <returns></returns>
    public async Task<TResponse> Get<TResponse>(string url) where TResponse : class
    {
        //TODO: Add Guard clauses
        return await Request<TResponse>(HttpMethod.Get, url);
    }
   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="method"></param>
    /// <param name="query"></param>
    /// <param name="body"></param>
    /// <param name="formUrlEncoded"></param>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<TResponse> Request<TResponse>(HttpMethod method,
        string query,
        object? body = null,
        bool  formUrlEncoded = false)
        where TResponse : class
    {
        // TODO: Add Guard clauses
        try
        {
            var request = BuildRequestMessage(method,  BuildUri(query) , body, formUrlEncoded);
            await _authentication.AddAuthentication(_client, request);
            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
       
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(content) ?? throw new InvalidOperationException
            {
                HelpLink = null,
                HResult = 0,
                Source = null
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
   /// <summary>
   /// 
   /// </summary>
   /// <param name="url"></param>
   /// <returns></returns>
    private Uri BuildUri(string query)
    {
        if(!query.StartsWith("http") && !string.IsNullOrEmpty(_baseUrl))
        {
            return new Uri( $"{_baseUrl}/{query.TrimStart('/')}");
        }
        
        return new Uri(query);
    }
   
  /// <summary>
  /// 
  /// </summary>
  /// <param name="method"></param>
  /// <param name="url"></param>
  /// <param name="body"></param>
  /// <param name="formUrlEncoded"></param>
  /// <returns></returns>
   private HttpRequestMessage BuildRequestMessage(HttpMethod method, Uri url, object? body,  bool  formUrlEncoded = false)
   {
      return body == null ? new HttpRequestMessage(method, url) : new HttpRequestMessage(method, url) {Content = BuildContent(body, formUrlEncoded)};
   }
   
  /// <summary>
  /// 
  /// </summary>
  /// <param name="body"></param>
  /// <param name="formUrlEncoded"></param>
  /// <returns></returns>
  private HttpContent BuildContent(object body, bool formUrlEncoded)
   {
       return formUrlEncoded ? BuildEncodedContent(body) : BuildJsonContent(body);
   }
  
  /// <summary>
  /// 
  /// </summary>
  /// <param name="body"></param>
  /// <returns></returns>
  private FormUrlEncodedContent BuildEncodedContent(object body)
   {
       var parameters = new Dictionary<string, string>();
       
         foreach(var property in body.GetType().GetProperties())
         {
             if (property.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false).FirstOrDefault() is not
                 JsonPropertyNameAttribute jsonAttr) continue;
             if(property.GetValue(body) != null) parameters.Add(jsonAttr.Name, property.GetValue(body)?.ToString() ?? string.Empty);
         }

         return new FormUrlEncodedContent(parameters);
   }
    
  
  /// <summary>
  /// 
  /// </summary>
  /// <param name="body"></param>
  /// <returns></returns>
  private StringContent BuildJsonContent(object? body)
   {
       return new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, MediaTypeNames.Application.Json);
   }
}
