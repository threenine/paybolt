namespace BoltPay.Authentication;

public class NoAuthentication : AuthenticationBase
{
    public override Task AddAuthentication(HttpClient client, HttpRequestMessage request)
    {
        return Task.CompletedTask;
    }
}