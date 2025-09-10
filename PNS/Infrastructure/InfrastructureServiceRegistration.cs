// File Path: Infrastructure/InfrastructureServiceRegistration.cs

using Application.Contracts;
using Application.Common.Interfaces;
using Application.Services;
using Infrastructure.BackgroundServices;
using Infrastructure.Caching;
using Infrastructure.Email;
using Infrastructure.Email.Providers;
using Infrastructure.Services;
using Infrastructure.Sms; // ይህን ያክሉ
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Other registrations...

            // Email Services
            services.AddSingleton<EmailQueueProcessor>();
            services.AddHostedService(provider => provider.GetRequiredService<EmailQueueProcessor>());
            services.AddScoped<IEmailQueueService, EmailQueueService>();
            services.AddScoped<IEmailService, EnhancedEmailService>();
            services.AddScoped<IEmailProvider, SmtpEmailProvider>();

            // SMS Services
            services.AddScoped<ISmsService, EnhancedSmsService>();
            services.AddScoped<ISmsProvider, TwilioSmsProvider>(); // የ Twilio አቅራቢን ያስመዝግቡ

            services.AddSingleton<IDateTime, DateTimeService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();

            return services;
        }
    }
}