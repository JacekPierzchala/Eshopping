using Discount.Api.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extensions;
using Discount.Infrastructure.Repositories;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(typeof(CreateDiscountCommandHandler).GetTypeInfo().Assembly);
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddGrpc();

var app = builder.Build();

app.MigrateDatabase<Program>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.UseEndpoints(endpoints => 
{
    endpoints.MapGrpcService<DiscountService>();
    endpoints.MapGet("/", async context => 
    {
        await context.Response.WriteAsync(
            "Communication with gRPC endpoints must be made through a gRPC client");
    });
});
app.Run();
