using BoltPay.Authentication;
using PayBolt.DependencyInjection;

namespace BoltPay.Lnd.Authentication;

public class MacaroonAuthentication : AuthenticationBase
{
    private const string HEADER_KEY= "Grpc-Metadata-macaroon";
    private readonly byte[] _macaroon;

    public MacaroonAuthentication(byte[] macaroon)
    {
        _macaroon = macaroon;
    }
    
    public override Task AddAuthentication(HttpClient client, HttpRequestMessage request)
    {
       
        request.Headers.Add(HEADER_KEY, _macaroon.ToBitString());
        return Task.CompletedTask;
    }
}