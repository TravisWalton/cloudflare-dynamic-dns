using System.Text.Json.Serialization;

namespace CloudflareDynamicDns.Models
{
    internal class UpdateDnsRecordRequest
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("comment")]
        public string Comment { get; set; }
    }
}
