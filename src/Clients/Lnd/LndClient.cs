using BoltPay.Authentication;
using BoltPay.Exceptions;
using BoltPay.Lightning;
using BoltPay.Lnd.Authentication;
using BoltPay.Lnd.Responses;
using BoltPay.Networking;
using BoltPay.Pay;
using PayBolt.DependencyInjection;

namespace BoltPay.Lnd;

public class LndClient : ServiceBase, ILightningClient, IDisposable
{
    private bool clientInternal = false;
    public LndClient(HttpClient client, LndOptions options ) : base( client, options.Address.ToBaseUrl(), BuildAuthentication(options))
    {
    }
    
    public static LndClient Create(string baseUrl,  string? macaroonHexString = null,
        byte[]? macaroonBytes = null,
        HttpClient? httpClient = null)
    {
       var internalBuild = false;
       
        if (httpClient == null)
        {
            httpClient = new HttpClient();
            internalBuild = true;
        }
        
        if(!Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri))
        {
            throw new PayBoltException($"Invalid uri format for LND Client : {baseUrl}");
        }
        
        var client = new LndClient(httpClient, new LndOptions
        {
            Address = new Uri(baseUrl),
            Macaroon = macaroonBytes ?? macaroonHexString.HexStringToByteArray()
        });
        
        client.clientInternal = internalBuild;
        return client;
    }

    internal static AuthenticationBase BuildAuthentication(LndOptions options)
    {
        return new MacaroonAuthentication(options.Macaroon);
    }
    
    public async Task<Connection> Connect()
    {
        try
        {
            var result = await Get<GetInfo>(Routes.GetInfo);
            if (string.IsNullOrEmpty(result.Alias))
            {
                return new Connection(Result.Error, "Unable to connect to LND");
            }
        }
        catch (Exception e)
        {
            return new Connection(Result.Error, e.Message);
        }

        return new Connection(Result.Ok);
    }

    public async Task<Currency> Balance()
    {
        var result = await Get<Balance>(Routes.Balance);
        var sats = result?.Total ?? 0;
        return Currency.FromSats(sats);
    }

    public Task<Invoice> Create(Currency amount, string description, Options? options = null)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Paid(string identifier)
    {
        throw new NotImplementedException();
    }

    public Task<PaymentResponse> Pay(string paymentRequest)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}