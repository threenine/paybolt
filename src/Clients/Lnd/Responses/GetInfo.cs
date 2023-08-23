using System.Text.Json.Serialization;

namespace BoltPay.Lnd.Responses;

public class GetInfo
{
  [JsonPropertyName("alias")]
  public string Alias { get; set; }
}