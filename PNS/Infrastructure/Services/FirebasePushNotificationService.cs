using Application.Contracts;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class FirebasePushNotificationService : IPushNotificationService
    {
        private readonly ILogger<FirebasePushNotificationService> _logger;
        private readonly bool _isInitialized = false;

        public FirebasePushNotificationService(IConfiguration configuration, ILogger<FirebasePushNotificationService> logger)
        {
            _logger = logger;
            try
            {
                if (FirebaseApp.DefaultInstance == null)
                {
                    var path = configuration["Firebase:ServiceAccountFilePath"];
                    if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    {
                        FirebaseApp.Create(new AppOptions()
                        {
                            Credential = GoogleCredential.FromFile(path),
                        });
                        _isInitialized = true;
                    }
                    else
                    {
                        _logger.LogWarning("Firebase Service Account file not found. Push notifications will not work.");
                    }
                }
                else
                {
                    _isInitialized = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing Firebase Admin SDK");
            }
        }

        public async Task<bool> SendPushAsync(string token, string title, string body, Dictionary<string, string>? data = null)
        {
            if (!_isInitialized) return false;

            try
            {
                var message = new Message()
                {
                    Token = token,
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body,
                    },
                    Data = data
                };

                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                _logger.LogInformation("Successfully sent push message: {Response}", response);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending push notification to token {Token}", token);
                return false;
            }
        }

        public async Task<int> SendBatchPushAsync(List<string> tokens, string title, string body, Dictionary<string, string>? data = null)
        {
            if (!_isInitialized || tokens == null || tokens.Count == 0) return 0;

            try
            {
                var message = new MulticastMessage()
                {
                    Tokens = tokens,
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body,
                    },
                    Data = data
                };

                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                _logger.LogInformation("Successfully sent {Count} push messages. Failures: {Failures}", 
                    response.SuccessCount, response.FailureCount);
                
                return response.SuccessCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending batch push notifications");
                return 0;
            }
        }
    }
}
