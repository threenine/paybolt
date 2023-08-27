using PayBolt;
using PayBolt.DependencyInjection;
using PayBolt.Lightning;

namespace PayBolt.Clients.Lnd.Contracts.v1.Responses;

internal static class Extensions
{
    public static Invoice ToLightningInvoice(this AddInvoice source,
        Currency amount,
        string memo,
        Options options)
    {

        return new Invoice
        {
            Id = source.RHash.ToBitString(),
            Memo = memo,
            Amount = amount,
            BOLT11 = source.PaymentRequest,
            Status = InvoiceStatus.Unpaid,
            ExpiresAt = options.ToExpiryDate()
        };
    }

}