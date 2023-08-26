using System.Text.Json.Serialization;

namespace BoltPay.Clients.Lnd.Contracts.v1.Requests;

public class LnrpcInvoice
{
    [JsonPropertyName("memo")]
    public string Memo { get; set; }

    [JsonPropertyName("receipt")]
    public byte[] Receipt { get; set; }

    [JsonPropertyName("r_preimage")]
    public byte[] R_preimage { get; set; }

    [JsonPropertyName("r_hash")]
    public byte[] R_hash { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("amt_paid_msat")]
    public string AmountPaid { get; set; }

    [JsonPropertyName("settled")]
    public bool? Settled { get; set; }

    [JsonPropertyName("creation_date")]
    public string Creation_date { get; set; }

    [JsonPropertyName("settle_date")]
    public string Settle_date { get; set; }

    [JsonPropertyName("payment_request")]
    public string Payment_request { get; set; }

    [JsonPropertyName("description_hash")]
    public byte[] Description_hash { get; set; }

    [JsonPropertyName("expiry")]
    public string Expiry { get; set; }


    [JsonPropertyName("fallback_addr")]
    public string Fallback_addr { get; set; }


    [JsonPropertyName("cltv_expiry")]
    public string Cltv_expiry { get; set; }

    [JsonPropertyName("private")]
    public bool? Private { get; set; }
}