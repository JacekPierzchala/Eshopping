using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Catalog.Infrastucture;
using Catalog.Infrastucture.Repositories;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddApiVersioning();
builder.Services.AddHealthChecks()
    .AddMongoDb(builder.Configuration["DatabaseSettings:ConnectionString"],
    "Catalog Mongo Db Health checks", HealthStatus.Degraded);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(typeof(CreateProductCommandHandler).GetTypeInfo().Assembly);
builder.Services.AddScoped<ICatalogContext,CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITypeRpository, ProductRepository>();
builder.Services.AddScoped<IBrandRepository, ProductRepository>();

//identty
var userPolicy= new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddControllers(conf=>conf.Filters.Add(new AuthorizeFilter(userPolicy)));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt => 
    {
        opt.Authority = "https://localhost:9009";
        opt.Audience = "Catalog";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();

app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse

});
app.Run();
