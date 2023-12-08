using Catalog.Core.Entities;
using Catalog.Infrastucture.Data;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver.Core.Clusters;

namespace Catalog.Infrastucture;

public class CatalogContext : ICatalogContext
{
    public CatalogContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

        if(client.Cluster.Description.State==ClusterState.Disconnected)
        {
            ConfigureProductMapping();
        }


        Brands = database.GetCollection<ProductBrand>(configuration.GetValue<string>("DatabaseSettings:BrandsCollection"));
        Types = database.GetCollection<ProductType>(configuration.GetValue<string>("DatabaseSettings:TypesCollection"));
        Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));

     

        BrandContextSeed.SeedData(Brands);
        TypeContextSeed.SeedData(Types);
        CatalogContextSeed.SeedData(Products);
    }

    private void ConfigureProductMapping()
    {
        BsonClassMap.RegisterClassMap<Product>(cm =>
        {
            cm.AutoMap();
            cm.SetIdMember(cm.GetMemberMap(c => c.Id));
            cm.MapMember(c => c.Name).SetElementName("Name");
            cm.MapMember(c => c.Price).SetSerializer(new DecimalSerializer(BsonType.Decimal128));
        });
        BsonClassMap.RegisterClassMap<ProductBrand>(cm =>
        {
            cm.AutoMap();
            cm.SetIdMember(cm.GetMemberMap(c => c.Id));
            cm.MapMember(c => c.Name).SetElementName("Name");
        });
        BsonClassMap.RegisterClassMap<ProductType>(cm =>
        {
            cm.AutoMap();
            cm.SetIdMember(cm.GetMemberMap(c => c.Id));
            cm.MapMember(c => c.Name).SetElementName("Name");

        });
    }
    public IMongoCollection<Product> Products { get; }

    public IMongoCollection<ProductBrand> Brands { get; }

    public IMongoCollection<ProductType> Types { get; }
}