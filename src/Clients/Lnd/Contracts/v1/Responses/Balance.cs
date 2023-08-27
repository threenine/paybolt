using System.Text.Json.Serialization;

namespace PayBolt.Clients.Lnd.Contracts.v1.Responses;

internal class Balance
{
    [JsonPropertyName("total_balance")]
    public long  Total { get; set; }
    
    [JsonPropertyName("confirmed_balance")]
    public long Confirmed { get; set; }
    
    [JsonPropertyName("unconfirmed_balance")]
    public long Unconfirmed { get; set; }
    
    [JsonPropertyName("locked_balance")]
    public long Locked { get; set; }
    
    [JsonPropertyName("account_balance")]
    public long Account { get; set; }
}