using System.Text.Json.Serialization;

namespace PayBolt.Clients.Lnd.Contracts.v1.Responses;

internal class GetInfo
{
    [JsonPropertyName("version")] 
    public string Version { get; set; }
    
    [JsonPropertyName("commit_hash")]
    public string CommitHash { get; set; }
    
    [JsonPropertyName("color")]
    public string Color { get; set; }
    
    [JsonPropertyName("identity_pubkey")]
    public string IdentityPubKey { get; set; }
    
    [JsonPropertyName("num_active_channels")]
    public int NumActiveChannels { get; set; }
    
    [JsonPropertyName("num_peers")]
    public int NumPeers { get; set; }
    
    [JsonPropertyName("block_height")] 
    public int BlockHeight { get; set; }
    
    [JsonPropertyName("block_hash")]
    public string BlockHash { get; set; }
    
    [JsonPropertyName("synced_to_chain")]
    public bool SyncedToChain { get; set; }
    
    [JsonPropertyName("synced_to_graph")]
    public bool SyncedToGraph { get; set; }
    
    [JsonPropertyName("chains")]
    public string[] Chains { get; set; }
    
    [JsonPropertyName("uris")]
    public string[] Uris { get; set; }
    
    [JsonPropertyName("best_header_timestamp")]
    public long BestHeaderTimestamp { get; set; }
    
    [JsonPropertyName("store_final_htlc_resolutions")]
    public bool StoreFinalHtlcResolutions { get; set; }
    
    [JsonPropertyName("alias")]
    public string Alias { get; set; }
}