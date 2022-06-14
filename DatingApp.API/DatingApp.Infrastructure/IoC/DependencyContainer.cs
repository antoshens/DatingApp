using DatingApp.Core.Data;
using DatingApp.Business.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DatingApp.Business.Services.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using DatingApp.Core.Model.AutoMapper;
using DatingApp.Core.Bus;
using DatingApp.Infrastructure.Bus;
using DatingApp.Business.EventHandlers;
using DatingApp.Business.Events;
using DatingApp.Core.Model;
using Microsoft.AspNetCore.Identity;
using DatingApp.Business.Model;
using DatingApp.Business.CQRS;

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

            // Identity setup
            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddSignInManager<SignInManager<User>>()
                .AddRoleValidator<RoleValidator<Role>>()
                .AddEntityFrameworkStores<DataContext>();

            // DatingApp.Business
            /* Register services */
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICurrentUser, CurrentUser>();

            /* Unit of work */
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            /* CQRS Mediator */
            services.AddScoped<ICQRSMediator, CQRSMediator>(sp =>
            {
                return new CQRSMediator(sp);
            });

            /* Queries and Commands */
            var business = typeof(ICQRSMediator).Assembly;
            var cqrsTypes = business.GetTypes().Where(t => (typeof(ICommandHandler).IsAssignableFrom(t)
                    || typeof(IQueryHandler).IsAssignableFrom(t))
                    && t != typeof(ICommandHandler) && t != typeof(IQueryHandler));

            foreach (Type cqrsTypeToRegister in cqrsTypes)
            {
                services.AddScoped(cqrsTypeToRegister);
            }

            services.AddSignalR();

            // Event Bus Subscriptions
            services.AddTransient<PhotoUploadedEventHandler>();
            services.AddTransient<IEventHandler<PhotoUploadedEvent>, PhotoUploadedEventHandler>();

            services.AddTransient<PhotoDeletedEventHandler>();
            services.AddTransient<IEventHandler<PhotoDeletedEvent>, PhotoDeletedEventHandler>();

            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var logger = sp.GetRequiredService<ILogger<RabbitMQBus>>();
                var rabbitMQOptions = sp.GetRequiredService<RabbitMQOptions>();

                return new RabbitMQBus(scopeFactory, logger, rabbitMQOptions);
            });
        }
    }
}
