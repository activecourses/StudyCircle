using Database;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class Business
    {
        public static void AddBusinessService(this IServiceCollection services)
        {
            services.AddDatabaseService();
        }
    }
}
