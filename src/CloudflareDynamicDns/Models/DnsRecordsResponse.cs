using System.Text.Json.Serialization;

namespace CloudflareDynamicDns.Models
{
    internal class DnsRecordsResponse
    {
        public List<DnsResults> Result { get; set; } = [];
        public bool Success { get; set; }
    }

    internal class DnsResults
    {
        public required string Id { get; set; }
        [JsonPropertyName("zone_id")]
        public required string ZoneId { get; set; }
        [JsonPropertyName("zone_name")]
        public required string ZoneName { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public string Content { get; set; }
        public bool Proxiable { get; set; }
        public bool Proxied { get; set; }
        public string Comment { get; set; }
        [JsonPropertyName("created_on")]
        public string CreatedOn { get; set; }
        [JsonPropertyName("modified_on")]
        public string ModifiedOn { get; set; }
    }
}
