using DatingApp.Core.Data;
using DatingApp.Business.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DatingApp.Business.Services.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            /* Register DataContextFactory */
            services.AddScoped<DataContextFactory>();

            // DatingApp.Core
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                   .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                   .Options;

            using (var scope = services.BuildServiceProvider())
            {
                var logger = scope.GetRequiredService<ILogger<DataContext>>();

                /* MS SQL Server connection */
                var dataContextFactory = scope.GetRequiredService<DataContextFactory>();
                services.AddScoped<DataContext>(_ => dataContextFactory.CreateDataContext(contextOptions));
            }

            // DatingApp.Business
            /* Register services */
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
