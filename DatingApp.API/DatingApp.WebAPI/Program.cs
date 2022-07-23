using DatingApp.Business.EventHandlers;
using DatingApp.Business.Events;
using DatingApp.Business.Model;
using DatingApp.Core.Bus;
using DatingApp.Infrastructure.IoC;
using DatingApp.WebAPI.Middlewares;
using DatingApp.WebAPI.SignalR;
using DatingApp.WebAPI.Utils.Extensions;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

configuration["EnvironmentName"] = builder.Environment.EnvironmentName;
var origins = configuration["Origins"];

// Add services to the container.
RegisterServices(builder.Services, configuration);

BuildConfigurationOptions(builder.Services, configuration);

builder.Services.AddAuthentication(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

ConfigureEventBus(app);

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(_ => _.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins(origins.Split(";"))); 

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();

app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<PresenceHub>("hubs/message");

app.Run();

static void RegisterServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers().AddOData(options => options
        .Filter()
        .SetMaxTop(100)
        .SkipToken()
        .Count()
        .OrderBy());

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // Register services using DI container
    DependencyContainer.RegisterServices(services, configuration);

    services.AddSingleton<PresenceTracker>();
}

static void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    eventBus.Subscribe<PhotoUploadedEvent, PhotoUploadedEventHandler>();
    eventBus.Subscribe<PhotoDeletedEvent, PhotoDeletedEventHandler>();
}

static void BuildConfigurationOptions(IServiceCollection services, IConfiguration configuration)
{
    var rabbitMQConfiguration = configuration.GetSection("RabbitMQ");

    services.AddSingleton<RabbitMQOptions>(sp => new RabbitMQOptions
    {
        HostName = rabbitMQConfiguration.GetValue<string>("HostName"),
        Port = rabbitMQConfiguration.GetValue<int>("Port")
    });
}
