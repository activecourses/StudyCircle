using Database;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class Business
    {
        public static IServiceCollection AddBusinessService(this IServiceCollection services)
        {
            services.AddDatabaseService();

            return services;
        }
    }
}
