using Application.Contracts.IRepository;
using Application.Profiles;
using MediatR;
using Persistence.Repositories;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// የPersistence አገልግሎቶች ምዝገባ
builder.Services.AddDbContext<PnsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PnsConnectionString")));

// የRepositoryዎች ምዝገባ
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IApplicationNotificationTypeMapRepository, ApplicationNotificationTypeMapRepository>();

// የApplication አገልግሎቶች ምዝገባ (MediatR እና Validatorዎችን ጨምሮ)
builder.Services.AddApplicationServices();

// የAPI እና Swagger ምዝገባ
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();