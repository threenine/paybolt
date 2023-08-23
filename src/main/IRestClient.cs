namespace BoltPay;

public interface IRestClient
{
    Task<TResponse> Get<TResponse>(string url) where TResponse : class;
    
    Task<TResponse> Post<TResponse>(string url, object? body = null, bool formUrlEncoded = false)
        where TResponse : class;

    Task<TResponse> Request<TResponse>(HttpMethod method, string query, object? body = null, bool formUrlEncoded = false)
        where TResponse : class;
}