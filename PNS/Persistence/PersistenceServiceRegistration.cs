// File Path: Persistence/PersistenceServiceRegistration.cs
using Application.Contracts.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Interceptors;
using Application.Common.Interfaces; // ይህን ያክሉ
using Microsoft.AspNetCore.Identity;
using Domain.Models;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Interceptors ን ለመመዝገብ
            services.AddScoped<AuditableEntityInterceptor>();
            services.AddScoped<DomainEventInterceptor>();

            services.AddDbContext<PnsDbContext>((sp, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("PnsConnectionString"));
                options.AddInterceptors(
                    sp.GetRequiredService<AuditableEntityInterceptor>(),
                    sp.GetRequiredService<DomainEventInterceptor>()
                );
            });

            services.AddIdentityCore<AppUser>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<IdentityRole>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddEntityFrameworkStores<PnsDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register all your specific repositories
            services.AddScoped<IClientApplicationRepository, ClientApplicationRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationHistoryRepository, NotificationHistoryRepository>();
            services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
            services.AddScoped<IApplicationNotificationTypeMapRepository, ApplicationNotificationTypeMapRepository>();
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddScoped<IPriorityRepository, PriorityRepository>();
            services.AddScoped<ISmsTemplateRepository, SmsTemplateRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}