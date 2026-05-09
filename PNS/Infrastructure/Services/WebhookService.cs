using Application.Contracts;
using Application.DTO.Webhook;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class WebhookService : IWebhookService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WebhookService> _logger;

        public WebhookService(HttpClient httpClient, ILogger<WebhookService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task SendWebhookAsync(string url, string secret, WebhookPayload payload)
        {
            try
            {
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Add HMAC Signature if secret is provided
                if (!string.IsNullOrEmpty(secret))
                {
                    var signature = ComputeSignature(json, secret);
                    content.Headers.Add("X-PNS-Signature", signature);
                }

                _logger.LogInformation("Sending webhook to {Url} for event {Event}", url, payload.EventType);
                
                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Webhook to {Url} failed with status {Status}", url, response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending webhook to {Url}", url);
            }
        }

        private string ComputeSignature(string payload, string secret)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);
            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(payloadBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
