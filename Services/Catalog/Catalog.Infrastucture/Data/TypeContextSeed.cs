using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json;

namespace Catalog.Infrastucture.Data;

internal class TypeContextSeed
{
    public static void SeedData(IMongoCollection<ProductType> typeCollection)
    {
        bool checkTypes = typeCollection.Find(b => true).Any();
        //string path = Path.Combine("Data", "SeedData", "types.json");
        // string path = "../Catalog.Infrastucture/Data/SeedData/types.json";
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Data", "SeedData", "types.json");
        if (!checkTypes)
        {
            var typesData = File.ReadAllText(path);
            var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
            if (types != null)
            {
                foreach (var type in types)
                {
                    typeCollection.InsertOneAsync(type);
                }
            }
        }
    }
}
