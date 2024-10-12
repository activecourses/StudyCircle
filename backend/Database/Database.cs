using Microsoft.Extensions.DependencyInjection;

namespace Database
{
    public static class Database
    {
        public static void AddDatabaseService(this IServiceCollection services)
        {
            // Add the DbContext
            services.AddDbContext<AppDbContext>();
        }
    }
}

