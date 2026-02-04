using API.Middleware;
using Application;
using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Application.Contracts;
using Application.Contracts.IRepository;
using Application.Services;
using FluentValidation;
using Infrastructure.BackgroundServices;
using Infrastructure.Caching;
using Infrastructure.Email;
using Infrastructure.Email.Providers;
using Infrastructure.Services;
using Infrastructure.Sms;
using MediatR;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Interceptors;
using Persistence.Repositories;
using System.Threading.RateLimiting;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/pns-api-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// Database and Persistence Services (DbContext, Identity, Repositories, Interceptors)
builder.Services.ConfigurePersistenceServices(builder.Configuration);

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
builder.Services.AddScoped<Application.Contracts.IDashboardHubService, API.Services.DashboardHubService>();
builder.Services.AddTransient<Application.Contracts.Identity.IAuthService, Infrastructure.Identity.AuthService>();
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IDateTime, DateTimeService>();
builder.Services.AddScoped<IDomainEventService, DomainEventService>();

// Email Services - Use the SmtpSettings from Infrastructure.Email.Providers
builder.Services.Configure<Infrastructure.Email.Providers.SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<EnhancedEmailService>();

// Conditionally register email providers
var sendGridSection = builder.Configuration.GetSection("SendGridSettings");
if (!string.IsNullOrEmpty(sendGridSection["ApiKey"]))
{
    builder.Services.Configure<SendGridSettings>(sendGridSection);
    builder.Services.AddScoped<IEmailProvider, SendGridEmailProvider>();
}
else
{
    // Register SMTP provider only if SendGrid isn't configured
    builder.Services.AddScoped<IEmailProvider, SmtpEmailProvider>();
}
builder.Services.AddScoped<Application.Contracts.IEmailService, EnhancedEmailService>();

// SMS Services
builder.Services.AddScoped<ISmsService, Infrastructure.Services.SmsService>();
builder.Services.AddScoped<ISmsQueueService, SmsQueueService>();
builder.Services.AddScoped<EnhancedSmsService>();

// Background Services
builder.Services.AddScoped<IEmailQueueService, EmailQueueService>();
builder.Services.AddSingleton<EmailQueueProcessor>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<EmailQueueProcessor>());

// SMS Background Services
builder.Services.AddSingleton<SmsQueueProcessor>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<SmsQueueProcessor>());

// Caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
});
builder.Services.AddScoped<ICacheService, RedisCacheService>();

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
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000") // Add your frontend URLs here
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Required for SignalR with tokens
    });
});

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<PnsDbContext>("PnsDbContext-check")
    .AddCheck("email_service", () => HealthCheckResult.Healthy("Email service is running"))
    .AddCheck("sms_service", () => HealthCheckResult.Healthy("SMS service is running"));

// HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

// SignalR
builder.Services.AddSignalR();

// Authentication and Authorization
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "MySuperSecretKeyForDevelopmentOnly12345!"))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Push Notification Service API",
        Version = "v1",
        Description = "Enhanced Push Notification Service with advanced email and SMS capabilities"
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

app.UseCors("AllowAll");

app.UseHttpsRedirection();

// Enhanced Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "no-referrer");
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; font-src 'self'; connect-src 'self' http://localhost:5217 ws://localhost:5217;");
    await next();
});

// Serve static files for React frontend
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Fallback to React index.html for non-API routes
app.MapFallbackToFile("index.html");

// Health check endpoint
app.MapHealthChecks("/health");

app.MapHub<API.Hubs.NotificationHub>("/hubs/notification");

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        await SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();

