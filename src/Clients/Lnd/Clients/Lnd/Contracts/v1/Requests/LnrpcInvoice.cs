using System.Text.Json.Serialization;

namespace BoltPay.Clients.Lnd.Contracts.v1.Requests;

public class LnrpcInvoice
{
    [JsonPropertyName("memo")]
    public string Memo { get; set; }

    [JsonPropertyName("receipt")]
    public byte[] Receipt { get; set; }

    [JsonPropertyName("r_preimage")]
    public byte[] RPreimage { get; set; }

    [JsonPropertyName("r_hash")]
    public byte[] RHash { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("amt_paid_msat")]
    public string AmountPaid { get; set; }

    [JsonPropertyName("settled")]
    public bool? Settled { get; set; }

    [JsonPropertyName("creation_date")]
    public string CreationDate { get; set; }

    [JsonPropertyName("settle_date")]
    public string SettleDate { get; set; }

    [JsonPropertyName("payment_request")]
    public string PaymentRequest { get; set; }

    [JsonPropertyName("description_hash")]
    public byte[] DescriptionHash { get; set; }

    [JsonPropertyName("expiry")]
    public string Expiry { get; set; }


    [JsonPropertyName("fallback_addr")]
    public string FallbackAddress { get; set; }


    [JsonPropertyName("cltv_expiry")]
    public string CltvExpiry { get; set; }

    [JsonPropertyName("private")]
    public bool? Private { get; set; }
}