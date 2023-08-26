using BoltPay;
using BoltPay.Clients.Lnd.Contracts.v1.Responses;
using BoltPay.Lightning;
using PayBolt.DependencyInjection;

namespace PayBolt.Clients.Clients.Lnd.Contracts.v1.Responses;

internal static class Extensions
{
    public static Invoice ToLightningInvoice(this AddInvoice source,
        Currency amount,
        string memo,
        Options options)
    {

        return new Invoice
        {
            Id = source.Hash.ToBitString(),
            Memo = memo,
            Amount = amount,
            BOLT11 = source.PaymentRequest,
            Status = InvoiceStatus.Unpaid,
            ExpiresAt = options.ToExpiryDate()
        };
    }

}