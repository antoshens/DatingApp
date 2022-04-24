using PhotoService.Core.Bus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PhotoService.Business.Util;
using PhotoService.Business.Interfaces;
using PhotoService.Business.EventHandlers;
using PhotoService.Business.Events;

namespace PhotoService.Infrastructure
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            // Subscriptions 
            services.AddTransient<PhotoAddedEventHandler>();
            services.AddTransient<IEventHandler<PhotoAddedEvent>, PhotoAddedEventHandler>();

            services.AddTransient<DeletePhotoEventHandler>();
            services.AddTransient<IEventHandler<DeletePhotoEvent>, DeletePhotoEventHandler>();

            // Register services
            services.AddScoped<IPhotoService, Business.Services.PhotoService>();

            // Domain Bus
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var rabbitMQOptions = sp.GetRequiredService<RabbitMQOptions>();

                return new RabbitMQBus(scopeFactory, rabbitMQOptions);
            });
        }
    }
}