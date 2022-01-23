using DatingApp.Core.Data;
using DatingApp.Business.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DatingApp.Business.Services.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using DatingApp.Core.Data.Repositories;
using DatingApp.Core.Model.AutoMapper;
using DatingApp.Core.Bus;
using DatingApp.Infrastructure.Bus;
using DatingApp.Business.EventHandlers;
using DatingApp.Business.Events;

namespace DatingApp.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            /* Register DataContextFactory */
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<DataContextFactory>(_ => new DataContextFactory(
                        _.GetRequiredService<IHttpContextAccessor>(),
                        _.GetRequiredService<ILogger<DataContext>>()
                    ));

            // DatingApp.Core
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                   .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                   .Options;

            using (var scope = services.BuildServiceProvider())
            {
                /* MS SQL Server connection */
                var dataContextFactory = scope.GetRequiredService<DataContextFactory>();
                services.AddScoped<DataContext>(_ => dataContextFactory.CreateDataContext(contextOptions));
            }

            // AutoMapper
            var mapperConfig = AutoMapperProfile.RegisterMappings();

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            // DatingApp.Business
            /* Register services */
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IPhotoService, PhotoService>();

            // Event Bus Subscriptions
            services.AddTransient<PhotoUploadedEventHandler>();
            services.AddTransient<IEventHandler<PhotoUploadedEvent>, PhotoUploadedEventHandler>();

            services.AddTransient<PhotoDeletedEventHandler>();
            services.AddTransient<IEventHandler<PhotoDeletedEvent>, PhotoDeletedEventHandler>();

            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var logger = sp.GetRequiredService<ILogger<RabbitMQBus>>();

                return new RabbitMQBus(scopeFactory, logger);
            });
        }
    }
}
