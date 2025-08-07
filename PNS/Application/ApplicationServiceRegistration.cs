using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Application.CQRS.ClientApplication.Queries;
using FluentValidation;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // MediatRን በትክክለኛው Assembly ውስጥ እንዲመዘግብ
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetClientApplicationsListQuery).Assembly));

            // AutoMapperን መመዝገብ
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // FluentValidationን መመዝገብ
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}