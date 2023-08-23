namespace BoltPay;

public class Currency : IComparable, IComparable<Currency>, IEquatable<Currency>
{
    
    public long Value { get; }

    public Currency(decimal amount, Unit unit)
    {
        Value = (long)(amount * (long)unit);
    }

    public decimal ToSats()
    {
        return ConvertTo(Unit.Satoshi);
    }
    
    public decimal ToBtc()
    {
        return ConvertTo(Unit.BTC);
    }
    
    public decimal ConvertTo(Unit unit)
    {
        return (decimal)Value / (long)unit;
    }
    
    public static Currency FromSats(long sats)
    {
        return new Currency(sats, Unit.Satoshi);
    }
    
    public static Currency FromMilliSats(decimal btc)
    {
        return new Currency(btc, Unit.MilliSatoshi);
    }
   
    public int CompareTo(object? obj)
    {
        if (obj == null)
            return 1;

        var currency = obj as Currency;

        return this.CompareTo(currency);
    }

    public int CompareTo(Currency? other)
    {
        return other == null ? 1 : this.Value.CompareTo(other.Value);
    }

    public bool Equals(Currency? other)
    {
        return other != null && Value.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is Currency value && Value.Equals(value.Value);
    }
}