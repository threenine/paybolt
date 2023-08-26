using System.Text.Json.Serialization;

namespace BoltPay.Clients.Lnd.Contracts.v1.Responses;

internal class GetInfo
{
    [JsonPropertyName("alias")]
    public string Alias { get; set; }
}