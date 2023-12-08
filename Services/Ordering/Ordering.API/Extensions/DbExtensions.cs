using AutoMapper.Configuration.Annotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions;

public static class DbExtensions
{
    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext,IServiceProvider> seeder)
        where TContext:DbContext
    {

        using (var scope = host.Services.CreateScope())
      {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation($"started db migration {typeof(TContext).Name}");
                CallSeeder(seeder, context, services);
                logger.LogInformation($"completed db migration {typeof(TContext).Name}");
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, $"error occued while migration {typeof(TContext).Name}");

            }
        }

        return host;
    }

    private static void CallSeeder<TContext>
        (Action<TContext, IServiceProvider> seeder, TContext? context, IServiceProvider services)
        where TContext : DbContext
    {
       context.Database.Migrate();
       seeder(context, services);
    }
}
