using PhotoService.Core.Bus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PhotoService.Business.Util;
using PhotoService.Business.Interfaces;
using PhotoService.Business.EventHandlers;
using PhotoService.Business.Events;
using Newtonsoft.Json;

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
            services.Configure<CloudinarySettings>(settings => {
                var cloudSettings = ConfigureCloudinarySettigns(config.GetSection("CloudinarySettings").Value);

                settings.CloudName = cloudSettings.CloudName;
                settings.ApiKey = cloudSettings.ApiKey;
                settings.ApiSecret = cloudSettings.ApiSecret;
            });
            services.AddScoped<IPhotoService, Business.Services.PhotoService>();

            // Domain Bus
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

                return new RabbitMQBus(scopeFactory);
            });
        }

        private static CloudinarySettings ConfigureCloudinarySettigns(string settingsJson)
        {
            var cloudinarySettings = JsonConvert.DeserializeObject<CloudinarySettings>(settingsJson);

            return cloudinarySettings ?? new CloudinarySettings();
        }
    }
}