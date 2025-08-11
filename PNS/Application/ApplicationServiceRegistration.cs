// File Path: Application/ApplicationServiceRegistration.cs
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.CQRS.Notification.Queries;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // MediatR ን በትክክል ለማስመዝገብ
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetHighPriorityNotificationsQuery).Assembly));

            return services;
        }
    }
}