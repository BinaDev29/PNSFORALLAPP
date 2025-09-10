// File Path: Infrastructure/Sms/TwilioSmsProvider.cs
using Application.Models.Sms;
using Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Infrastructure.Sms
{
    public class TwilioSmsProvider : ISmsProvider
    {
        public string Name => "Twilio";
        public int Priority => 1;
        private readonly IConfiguration _configuration;

        public TwilioSmsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<SmsSendResult> SendSmsAsync(SmsMessage smsMessage)
        {
            try
            {
                var accountSid = _configuration["Twilio:AccountSid"];
                var authToken = _configuration["Twilio:AuthToken"];
                var twilioPhoneNumber = _configuration["Twilio:PhoneNumber"];

                if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(twilioPhoneNumber))
                {
                    return SmsSendResult.Failure("Twilio credentials are not configured.", Name);
                }

                TwilioClient.Init(accountSid, authToken);

                var message = await MessageResource.CreateAsync(
                    body: smsMessage.Body,
                    // FIX: Explicitly use the Twilio.Types.PhoneNumber class
                    from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(smsMessage.To)
                );

                if (message.Status == MessageResource.StatusEnum.Queued || message.Status == MessageResource.StatusEnum.Sending || message.Status == MessageResource.StatusEnum.Sent)
                {
                    return SmsSendResult.Success(message.Sid, Name);
                }
                else
                {
                    return SmsSendResult.Failure(message.ErrorMessage ?? "Unknown error", Name);
                }
            }
            catch (Exception ex)
            {
                return SmsSendResult.Failure(ex.Message, Name);
            }
        }
    }
}