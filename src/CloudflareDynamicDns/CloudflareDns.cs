using System.Net.Http.Headers;
using System.Text.Json;
using CloudflareDynamicDns.Models;

namespace CloudflareDynamicDns
{
    internal class CloudflareDns
    {
        private readonly Config _config;

        public CloudflareDns(string configLocation = "config.json")
        {
            var jsonFile = File.ReadAllText(configLocation);
            _config = JsonSerializer.Deserialize<Config>(jsonFile, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) 
                      ?? throw new InvalidOperationException("Config invalid.");
        }

        public void VerifyToken()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config.ApiToken}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync("https://api.cloudflare.com/client/v4/user/tokens/verify").Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var verifyResponse = JsonSerializer.Deserialize<VerifyResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var json = JsonSerializer.Serialize(verifyResponse, new JsonSerializerOptions { WriteIndented = true });
            Console.Write(json);
        }

        public void DynamicallyUpdateDnsRecord()
        {
            var dnsRecordsResponse = GetDnsRecords();

            if (dnsRecordsResponse == null)
            {
                Console.WriteLine("Cloudflare did not return any DNS records.");
                return;
            }

            var selectedDnsRecord = dnsRecordsResponse.Result.Find(x =>
                x.Name == _config.Domain && x.Type == "A" && x.ZoneId == _config.ZoneId);

            if (selectedDnsRecord == null)
            {
                Console.WriteLine("Could not find DNS record.");
                return;
            }

            var currentIpAddress = GetPublicIpAddress();

            var request = new UpdateDnsRecordRequest
            {
                Content = currentIpAddress,
                Comment = $"Updated by Cloudflare Dynamic DNS on {DateTime.Now:G}"
            };

            var results = UpdateDnsRecord(request, selectedDnsRecord.Id);

            if (results.IsSuccessStatusCode)
            {
                Console.WriteLine($"Setting IP Address From {selectedDnsRecord.Content} to {currentIpAddress} on DNS record {selectedDnsRecord.Name}.");

            }
            else
            {
                Console.WriteLine($"Failed to update DNS record with status code: {results.StatusCode}");
            }
        }

        public DnsRecordsResponse? GetDnsRecords()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Auth-Email", $"{_config.Email}");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config.ApiToken}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync($"https://api.cloudflare.com/client/v4/zones/{_config.ZoneId}/dns_records").Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var dnsRecordsResponse = JsonSerializer.Deserialize<DnsRecordsResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return dnsRecordsResponse;
        }

        public HttpResponseMessage UpdateDnsRecord(UpdateDnsRecordRequest request, string id)
        {
            var content = JsonSerializer.Serialize(request);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Auth-Email", $"{_config.Email}");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config.ApiToken}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response =
                client.PatchAsync($"https://api.cloudflare.com/client/v4/zones/{_config.ZoneId}/dns_records/{id}", new StringContent(content));
            var responseBody = response.Result;
            return responseBody;
        }

        public string GetPublicIpAddress()
        {
            try
            {
                using var client = new HttpClient();
                return client.GetStringAsync("https://api64.ipify.org").Result;
            }
            catch (Exception ex)
            {
                return $"Error getting public IP address: {ex.Message}";
            }
        }
    }
}
