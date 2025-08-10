// File Path: Persistence/PersistenceServiceRegistration.cs
using Application.Contracts.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PnsDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("PnsConnectionString")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register all your specific repositories
            services.AddScoped<IClientApplicationRepository, ClientApplicationRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationHistoryRepository, NotificationHistoryRepository>();
            services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
            services.AddScoped<IApplicationNotificationTypeMapRepository, ApplicationNotificationTypeMapRepository>();
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddScoped<IPriorityRepository, PriorityRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}