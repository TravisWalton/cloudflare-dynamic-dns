# Cloudflare Dynamic DNS
Simple .NET console app that updates an "A" record via Cloudflare's API.
## Overview
Pending...
## Setup
This app is intended to be run behind a task scheduler.

Create a config file named <code>config.json</code> and place it in the same dir as the app's exe file.
```json
{
  "email": "your-Cloudflare-email@email.com",
  "apiToken": "token",
  "zoneId": "zoneId",
  "recordName": "full-dns-record-name"
}
```
Description of config:
- *Email* - This is the email registered on your Cloudflare account.
- *API Token* - Used for authentication with Cloudflare's API. Here is a guide on how to <a href="https://developers.cloudflare.com/fundamentals/api/get-started/create-token/">create an API token</a>.
- *Zone Id* - Cloudflare has a specific DNS records for each Zone (domain name) on your account. The Zone Id identifies the correct Zone (domain name) so you update the right DNS record for the right domain.
- *Record Name* - Identifies the precise DNS "A" record you wish to update.
