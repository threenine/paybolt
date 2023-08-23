using System.Text.Json.Serialization;

namespace BoltPay.Lnd.Responses;

internal class Balance
{
    [JsonPropertyName("total_balance")]
    public long  Total { get; set; }
}