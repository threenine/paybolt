namespace BoltPay.Lightning;

public class Invoice
{
    /// <summary>
    /// Invoice Identifier.
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string Memo { get; set; }
    public InvoiceStatus Status { get; set; }
    public string BOLT11 { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public Currency Amount { get; set; }
    public string Uri => $"lightning:{BOLT11}";
}