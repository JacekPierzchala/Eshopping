using Microsoft.Extensions.Configuration;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data;
using Ordering.API.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MassTransit;
using Ordering.API.EventBusConsumer;
using EventBus.Messages.Common;

namespace Ordering.API;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning();
        services.AddApplicationServices();
        services.RegisterInfaServices(Configuration);
        services.AddAutoMapper(typeof(Startup));
        services.AddHealthChecks().Services.AddDbContext<OrderContext>();
        services.AddSwaggerGen();
        services.AddScoped<BasketOrderingConsumer>();
        services.AddEndpointsApiExplorer();

        services.AddMassTransit(config =>
        {
            //mark as consumer
            config.AddConsumer<BasketOrderingConsumer>();
            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(Configuration["EventBusSettings:HostAddress"]);

                //privide queue name
                cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c => 
                {
                    c.ConfigureConsumer<BasketOrderingConsumer>(context);
                });
            });
        });

        services.AddMassTransitHostedService();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
}

