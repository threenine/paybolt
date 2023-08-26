using System.Text.Json.Serialization;

namespace BoltPay.Clients.Lnd.Contracts.v1.Responses;

internal class Balance
{
    [JsonPropertyName("total_balance")]
    public long  Total { get; set; }
}