using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database
{
    public static class Database
    {
        public static IServiceCollection AddDatabaseService(this IServiceCollection services)
        {
            // Add the DbContext
            services.AddDbContext<AppDbContext>();

            return services;
        }
    }
}

