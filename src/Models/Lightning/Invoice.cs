using BoltPay.Currency;

namespace BoltPay.Lightning;

public class Invoice
{
    public string Id { get; set; }
    public string Memo { get; set; }
    public InvoiceStatus Status { get; set; }
    public string BOLT11 { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public Money Amount { get; set; }
    public string Uri => $"lightning:{BOLT11}";
}