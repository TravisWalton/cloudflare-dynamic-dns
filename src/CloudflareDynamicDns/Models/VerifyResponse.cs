using System.Text.Json.Serialization;

namespace CloudflareDynamicDns.Models
{
    internal class VerifyResponse
    {
        public Result Result { get; set; }
        public bool Success { get; set; }
        public List<object> Errors { get; set; }
        public List<Message> Messages { get; set; }
    }
    public class Result
    {
        public string Id { get; set; }
        public string Status { get; set; }
    }

    public class Message
    {
        public int Code { get; set; }
        [JsonPropertyName("message")]
        public string MessageText { get; set; }
        public string Type { get; set; }
    }
}
