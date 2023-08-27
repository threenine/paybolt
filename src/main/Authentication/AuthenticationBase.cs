namespace PayBolt.Authentication;

public abstract class AuthenticationBase
{
    public abstract Task AddAuthentication(HttpClient client, HttpRequestMessage request);
}