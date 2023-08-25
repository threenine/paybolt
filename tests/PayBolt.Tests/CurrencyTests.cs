using System.ComponentModel;
using Shouldly;

namespace BoltPay.Tests;

public class CurrencyConversionTests
{

    /// <summary>
    /// Currency conversion tests to ensure that the conversion is working as expected
    /// 
    /// </summary>

    [Fact, Description("A basic conversion of 1 Satoshi to 1000 Millisatoshi")]
    public void One_sat_should_be_be_equal_to_1000_millisats()
    {
        Currency.FromSats(1).ShouldBe(Currency.FromMilliSats(1000));
    }

    [Fact, Description("A basic conversion of 1 BTC to 1000 Milli BTC")]
    public void One_btc_should_be_be_equal_to_1000_millibtc()
    {
        new Currency(1, Unit.BTC).ShouldBe(new Currency(1000, Unit.MilliBTC));
    }

    [Fact, Description("A basic conversion of 1 BTC to 100000000 Satoshis")]
    public void One_btc_should_be_be_equal_to_100000000_satoshis()
    {
        new Currency(1, Unit.BTC).ShouldBe(Currency.FromSats(100000000));
    }

    [Fact, Description("A basic conversion of 1 Satoshi to 1000 Millisatoshi")]
    public void One_sat_should_be_be_equal_to_1000_millisats_using_convert_to()
    {
        var currency = Currency.FromSats(1);
        currency.ConvertTo(Unit.MilliSatoshi).ShouldBe(1000);
    }

    [Fact, Description("1 Sat should be equal to 1 Sat")]
    public void One_sat_should_be_be_equal_to_1_sat()
    {
        Currency.FromSats(1).ShouldBe(Currency.FromSats(1));
    }

    [Fact, Description("1 BTC should be 1 BTC")]
    public void One_btc_should_be_be_equal_to_1_btc()
    {
        new Currency(1, Unit.BTC).ShouldBe(new Currency(1, Unit.BTC));
    }

    [Fact, Description("1 Sat to BTC should be 0.00000001")]
    public void One_sat_should_be_less_than_1000_millisats()
    {
        new Currency(1, Unit.Satoshi).ToBtc().ShouldBeEquivalentTo(0.00000001M);
    }

    [Fact, Description("1 Sat to be equivalent to 1000 Millisats")]
    public void One_sat_should_be_equivalent_to_1000_millisats()
    {
        Currency.FromSats(1).Equals(Currency.FromMilliSats(1000)).ShouldBeTrue();
    }

    [Fact, Description("1 Sat should not be equal to  1 Millisats")]
    public void One_sat_should_not_be_equal_to_1_milli_sats()
    {
        Currency.FromSats(1).Equals(Currency.FromMilliSats(1)).ShouldBeFalse();
    }
    
    [Fact, Description("1000 Sat should be greater than  1000 Millisats")]
    public void One_thousand_sat_should_be_greater_than_1000_milli_sats()
    {
        Currency.FromSats(1000).ShouldBeGreaterThan(Currency.FromMilliSats(1000));
    }
    
    [Fact, Description("1000 Sat should be greater than or equal to  1000 Millisats")]
    public void One_thousand_sat_should_be_greater_than_or_equal_to_1000_milli_sats()
    {
        Currency.FromMilliSats(1000).ShouldBeLessThanOrEqualTo(Currency.FromSats(1));
    }
    
}