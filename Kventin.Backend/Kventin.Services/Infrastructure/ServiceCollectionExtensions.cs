using Kventin.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kventin.Services.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<KventinContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
