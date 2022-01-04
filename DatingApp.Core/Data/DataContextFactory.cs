using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DatingApp.Core.Data
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            // We can hard-code connection string, since IDesignTimeDbContextFactory is used mostly only for
            // designing reasons, and do not need for running migrations.
            // Hense this code will be running only in development environment
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS; Database=DatingApp_DB; Trusted_Connection=True;");

            return new DataContext(optionsBuilder.Options);
        }
    }
}
