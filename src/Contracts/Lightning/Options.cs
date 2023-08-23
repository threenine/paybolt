namespace BoltPay.Lightning;

public class Options
{
    public Options(TimeSpan? expiry = null)
    {
     Expiry = expiry;
    }

    public TimeSpan? Expiry { get; }
   
}