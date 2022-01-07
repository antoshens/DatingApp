using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DatingApp.Core.Data
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DataContext> _logger;

        /* Use for development purpose only */
        public DataContextFactory() : base()
        {

        }

        public DataContextFactory(IHttpContextAccessor httpContextAccessor,
            ILogger<DataContext> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public DataContext CreateDbContext(string[] args)
        {
            // We can hard-code connection string, since IDesignTimeDbContextFactory is used mostly only for
            // designing reasons, and do not need for running migrations.
            // Hense this code will be running only in development environment
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS; Database=DatingApp_DB; Trusted_Connection=True;");

            return new DataContext(optionsBuilder.Options);
        }

        public DataContext CreateDataContext(DbContextOptions<DataContext> options)
        {
            var currentUserId = GetCurrentUserId();

            var dataContext = new DataContext(options, _logger, currentUserId);

            return dataContext;
        }

        private int? GetCurrentUserId()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User;

            if (currentUser is null) return null;

            if (currentUser.HasClaim(c => c.Type == "sub"))
            {
                var userIdText = currentUser.Claims.First(c => c.Type == "sub").Value;
                var userId = Convert.ToInt32(userIdText);

                return userId;
            }

            return null;
        }
    }
}
