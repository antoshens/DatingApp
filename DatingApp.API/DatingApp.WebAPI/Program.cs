using DatingApp.Business.EventHandlers;
using DatingApp.Business.Events;
using DatingApp.Core.Bus;
using DatingApp.Infrastructure.IoC;
using DatingApp.WebAPI.Middlewares;
using DatingApp.WebAPI.SignalR;
using DatingApp.WebAPI.Utils.Extensions;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddAuthentication(builder.Configuration);

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
    .WithOrigins("http://localhost:4200")); // TODO: Use shared config file to store FE instance host address

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
