namespace PayBolt.DependencyInjection;

public static class UriExtensions
{
    
    public static string ToBaseUrl(this Uri source)
    {
        return source.ToString().TrimEnd('/');
    }
}