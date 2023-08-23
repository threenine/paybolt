using System.Text.Json.Serialization;

namespace BoltPay.Lnd.Responses;

internal class GetInfo
{
  [JsonPropertyName("alias")]
  public string Alias { get; set; }
}