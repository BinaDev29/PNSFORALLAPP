// File Path: Infrastructure/InfrastructureServiceRegistration.cs

using Application.Contracts;
using Application.Common.Interfaces;
using Application.Services;
using Infrastructure.BackgroundServices;
using Infrastructure.Caching;
using Infrastructure.Email;
using Infrastructure.Email.Providers;
using Infrastructure.Services;
using Infrastructure.Sms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Contracts.Identity;
using Infrastructure.Identity;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Email Services
            services.Configure<Infrastructure.Email.Providers.SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.Configure<Infrastructure.Email.Providers.SendGridSettings>(configuration.GetSection("SendGridSettings"));
            services.AddSingleton<EmailQueueProcessor>();
            services.AddHostedService(provider => provider.GetRequiredService<EmailQueueProcessor>());
            services.AddScoped<IEmailQueueService, EmailQueueService>();
            services.AddScoped<IEmailService, EnhancedEmailService>();
            services.AddScoped<IEmailProvider, SmtpEmailProvider>();

            // SMS Services
            services.AddScoped<ISmsService, EnhancedSmsService>();
            services.AddScoped<ISmsProvider, TwilioSmsProvider>();
            services.AddSingleton<ISmsQueueService, SmsQueueService>();
            services.AddSingleton<SmsQueueProcessor>();
            services.AddHostedService(provider => provider.GetRequiredService<SmsQueueProcessor>());

            // Other Services
            services.AddSingleton<IDateTime, DateTimeService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis") ?? "localhost:6379";
            });
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddHttpClient<IWebhookService, WebhookService>();
            services.AddSingleton<IPushNotificationService, FirebasePushNotificationService>();
            services.AddSingleton<IMessageQueueService, RabbitMqService>();

            return services;
        }
    }
}