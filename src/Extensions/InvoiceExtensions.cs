using System.Globalization;
using PayBolt.Lightning;

namespace PayBolt.DependencyInjection;

public static class InvoiceExtensions
{
    private static TimeSpan DEFAULT_EXPIRY = TimeSpan.FromDays(1);
    
    public static string ToExpiryString(this Options source)
    {
        TimeSpan expiry = DEFAULT_EXPIRY;

        if(source?.Expiry.HasValue == true)
        {
            expiry = source.Expiry.Value;
        }

        return Math.Round(expiry.TotalSeconds, 0).ToString(CultureInfo.InvariantCulture);
    }

    public static DateTimeOffset ToExpiryDate(this Options? source)
    {
        if (source is not { Expiry: not null })
        {
            return DateTimeOffset.UtcNow + DEFAULT_EXPIRY;
        }

        return DateTimeOffset.UtcNow + source.Expiry.Value;
    }
}