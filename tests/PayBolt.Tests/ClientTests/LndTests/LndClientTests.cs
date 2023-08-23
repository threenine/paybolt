using System.ComponentModel;
using BoltPay.Exceptions;
using BoltPay.Lnd;

namespace BoltPay.Tests.ClientTests.LndTests;

public class LndClientTests
{
    [Fact, Description("Should throw an exception if the URI is invalid")]
    public void ShouldThrowAnExceptionIfTheUriIsInvalid()
    {
       Assert.Throws<PayBoltException>(() => LndClient.Create("bad_uri"));
    }
}