using MediatR;
using PhotoService.Core.Bus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PhotoService.Business.Util;
using PhotoService.Business.Interfaces;

namespace PhotoService.Infrastructure
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            // Domain Bus
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory);
            });

            // Register services
            services.Configure<CloudinarySettings>(_ => { config.GetSection("CloudinarySettings"); });
            services.AddScoped<IPhotoService, Business.Services.PhotoService>();
        }
    }
}