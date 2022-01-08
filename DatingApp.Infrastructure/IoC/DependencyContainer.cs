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

            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            // DatingApp.Business
            /* Register services */
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICurrentUser, CurrentUser>();
        }
    }
}
