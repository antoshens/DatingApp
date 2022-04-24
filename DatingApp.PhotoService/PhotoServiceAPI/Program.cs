using PhotoService.Business.EventHandlers;
using PhotoService.Business.Events;
using MediatR;
using PhotoService.Core.Bus;
using PhotoService.Infrastructure;
using PhotoService.Business.Util;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration["EnvironmentName"] = builder.Environment.EnvironmentName;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(Program));

DependencyContainer.RegisterServices(builder.Services, builder.Configuration);
BuildConfigurationOptions(builder.Services, builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

ConfigureEventBus(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    eventBus.Subscribe<PhotoAddedEvent, PhotoAddedEventHandler>();
    eventBus.Subscribe<DeletePhotoEvent, DeletePhotoEventHandler>();
}

static void BuildConfigurationOptions(IServiceCollection services, IConfiguration configuration)
{
    var rabbitMQConfiguration = configuration.GetSection("RabbitMQSettings");
    services.AddSingleton<RabbitMQOptions>(_ => new RabbitMQOptions
    {
        HostName = rabbitMQConfiguration.GetValue<string>("HostName"),
        Port = rabbitMQConfiguration.GetValue<int>("Port")
    });

    var cloudinaryConfiguration = configuration.GetSection("CloudinarySettings");
    services.AddSingleton<CloudinarySettings>(_ => new CloudinarySettings
    {
        CloudName = rabbitMQConfiguration.GetValue<string>("CloudName"),
        ApiKey = rabbitMQConfiguration.GetValue<string>("ApiKey"),
        ApiSecret = rabbitMQConfiguration.GetValue<string>("ApiSecret")
    });
}