
using Basket.Application.Handlers;
using System.Reflection;
using MediatR;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Basket.Application.GrpcService;
using Discount.Grpc.Protos;
using MassTransit;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(typeof(CreateShoppingCartCommandHandler).GetTypeInfo().Assembly);
builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning();
builder.Services.AddStackExchangeRedisCache(opt => 
{
    opt.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

builder.Services.AddScoped<IBasketRepository,BasketRepository>();
builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
    (opt=> opt.Address=new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]) );

builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddRedis("CacheSettings:ConnectionString", "Redis Health", HealthStatus.Degraded);

builder.Services.AddMassTransit(config => 
{
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});

builder.Services.AddMassTransitHostedService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse

});

app.Run();
