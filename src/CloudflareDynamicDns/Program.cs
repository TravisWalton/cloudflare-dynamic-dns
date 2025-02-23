// See https://aka.ms/new-console-template for more information

using CloudflareDynamicDns;

Console.WriteLine("Starting Cloudflare Dynamic DNS");

var cloudflareDns = new CloudflareDns();

if (args.FirstOrDefault()?.ToLowerInvariant() == "auth")
{
    cloudflareDns.VerifyToken();
}
else
{
    cloudflareDns.DynamicallyUpdateDnsRecord();
}

Console.WriteLine("Completing Cloudflare Dynamic DNS");
