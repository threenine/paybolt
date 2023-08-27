using System.Text.Json.Serialization;

namespace PayBolt.Clients.Lnd.Contracts.v1.Responses;

internal class AddInvoice
{
    [JsonPropertyName("r_hash")] 
    public byte[] RHash { get; set; }
    
    [JsonPropertyName("payment_request")]
    public string PaymentRequest { get; set; }
    
    [JsonPropertyName("add_index")] 
    public string AddIndex { get; set; }
    
    [JsonPropertyName("payment_addr")]
    public string PaymentAddress { get; set; }
    
    
    
}