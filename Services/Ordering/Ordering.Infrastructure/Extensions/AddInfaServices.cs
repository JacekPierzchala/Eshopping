using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure;
public static class AddInfaServices
{
    public static IServiceCollection RegisterInfaServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<OrderContext>(opt => 
        {
            opt.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString"));
        });
        services.AddScoped(typeof(IAsyncRepository<>),typeof(RepositoryBase<>));
        services.AddScoped<IOrderRepository,OrderRepository>();

      
        return services;
    }
}
