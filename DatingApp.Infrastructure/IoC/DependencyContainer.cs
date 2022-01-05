using DatingApp.Core.Data;
using DatingApp.Business.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DatingApp.Business.Services.Authentication;

namespace DatingApp.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // DatingApp.Core
            services.AddDbContext<DataContext>(options =>
            {
                /* MS SQL Server connection */
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            // DatingApp.Business
            /* Register services */
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
