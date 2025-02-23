namespace CloudflareDynamicDns.Models
{
    internal class Config
    {
        public required string Email { get; set; }
        public required string ApiToken { get; set; }
        public required string ZoneId { get; set; }
        public required string Domain { get; set; }
    }
}
