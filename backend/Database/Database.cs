using Microsoft.Extensions.DependencyInjection;

namespace Database
{
    public static class Database
    {
        public static IServiceCollection AddDatabaseService(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();

            return services;
        }
    }
}