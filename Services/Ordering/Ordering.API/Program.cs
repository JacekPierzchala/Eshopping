using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data;
using Ordering.API.Extensions;
using Ordering.API;

internal class Program
{
    private static void Main(string[] args)
    {
        CreateHostBuilder(args)
       .Build()
       .MigrateDatabase<OrderContext>((context, services) =>
       {
           var logger = services.GetService<ILogger<OrderContextSeed>>();
           OrderContextSeed.SeedAsync(context, logger).Wait();
       }).Run();
    }
    private static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
}