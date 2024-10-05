using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        private static void DatabaseConfig(IServiceCollection services)
        {
            IConfigurationRoot conf = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = conf.GetConnectionString("DefaultConnection") ??
                                   throw new InvalidOperationException(
                                       "conndection string 'DefaultConnection' not found'");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}