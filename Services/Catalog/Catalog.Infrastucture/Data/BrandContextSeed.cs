using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json;

namespace Catalog.Infrastucture.Data;

internal static class BrandContextSeed
{
    public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
    {
        bool checkBrands = brandCollection.Find(b => true).Any();
        //string path = Path.Combine("Data", "SeedData", "brands.json");
        // var t= Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Data", "SeedData", "brands.json");

        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Data", "SeedData", "brands.json");
        if (!checkBrands)
        {
            var brandsData = File.ReadAllText(path);
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
            if (brands != null)
            {
                foreach (var brand in brands)
                {
                    brandCollection.InsertOneAsync(brand);
                }
            }
        }
    }
}
