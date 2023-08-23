using BoltPay.Configuration;

namespace BoltPay.Lnd;

public class LndOptions : Options
{
    public byte[] Macaroon { get; set; }
}