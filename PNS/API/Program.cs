using API.Middleware;
using Application;
using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Application.Contracts.IRepository;
using Application.Services;
using FluentValidation;
using Infrastructure.BackgroundServices;
using Infrastructure.Caching;
using Infrastructure.Email;
using Infrastructure.Email.Providers;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Interceptors;
using Persistence.Repositories;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<PnsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PnsConnectionString")));

// MediatR and Application Services
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceRegistration).Assembly);
});

// Add pipeline behaviors
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

// Application Services
builder.Services.ConfigureApplicationServices();
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IDateTime, DateTimeService>();
builder.Services.AddScoped<IDomainEventService, DomainEventService>();

// Email Services
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Only configure and register SendGrid provider when an ApiKey is present.
// This prevents DI from constructing SendGridEmailProvider with a null ApiKey and throwing ArgumentNullException.
var sendGridSection = builder.Configuration.GetSection("SendGridSettings");
if (!string.IsNullOrEmpty(sendGridSection["ApiKey"]))
{
    builder.Services.Configure<SendGridSettings>(sendGridSection);
    builder.Services.AddScoped<IEmailProvider, SendGridEmailProvider>();
}

// Ensure SMTP provider is registered so emails are sent via SMTP when SendGrid is not configured.
builder.Services.AddScoped<IEmailProvider, SmtpEmailProvider>();
builder.Services.AddScoped<EnhancedEmailService>();
builder.Services.AddScoped<Application.Contracts.IEmailService, EnhancedEmailService>();

// Background Services
builder.Services.AddScoped<IEmailQueueService, EmailQueueService>();
builder.Services.AddSingleton<EmailQueueProcessor>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<EmailQueueProcessor>());

// Caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
});
builder.Services.AddScoped<ICacheService, RedisCacheService>();

// Interceptors
builder.Services.AddScoped<AuditableEntityInterceptor>();
builder.Services.AddScoped<DomainEventInterceptor>();

// Register persistence repositories and UnitOfWork so DI can create handlers that depend on IUnitOfWork
builder.Services.AddScoped<IClientApplicationRepository, ClientApplicationRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationHistoryRepository, NotificationHistoryRepository>();
builder.Services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
builder.Services.AddScoped<IApplicationNotificationTypeMapRepository, ApplicationNotificationTypeMapRepository>();
builder.Services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
builder.Services.AddScoped<IPriorityRepository, PriorityRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("DefaultPolicy", configure =>
    {
        configure.PermitLimit = 100;
        configure.Window = TimeSpan.FromMinutes(1);
        configure.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        configure.QueueLimit = 50;
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<PnsDbContext>("PnsDbContext-check")
    .AddCheck("email_service", () => HealthCheckResult.Healthy("Email service is running"));

// HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Push Notification Service API",
        Version = "v1",
        Description = "Enhanced Push Notification Service with advanced email capabilities"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Show all controllers in Swagger
    c.DocInclusionPredicate((docName, apiDesc) => true);
    c.CustomSchemaIds(type => type.FullName);
});

// Logging
builder.Services.AddLogging(configure =>
{
    configure.AddConsole();
    configure.AddDebug();
});

var app = builder.Build();

// Always enable Swagger UI (not just in Development)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PNS API V1");
    // Remove or set RoutePrefix to "swagger" to serve at /swagger/index.html
    c.RoutePrefix = "swagger";
});

// Custom middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.MapHealthChecks("/health");

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PnsDbContext>();
    context.Database.EnsureCreated();
}

app.Run();