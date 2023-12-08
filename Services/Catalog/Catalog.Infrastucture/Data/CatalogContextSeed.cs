using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrastucture.Data;

internal class CatalogContextSeed
{
    public static void SeedData(IMongoCollection<Product> productsCollection)
    {
        bool checkProducts = productsCollection.Find(b => true).Any();
        //productsCollection.DeleteMany(c=>true);
        //string path = Path.Combine("Data", "SeedData", "products.json");
        //string path = "../Catalog.Infrastucture/Data/SeedData/products.json";
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Data", "SeedData", "products.json");
        if (!checkProducts)
        {
            var productsData = File.ReadAllText(path);
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            if (products != null)
            {
                foreach (var product in products)
                {
                    productsCollection.InsertOneAsync(product);
                }
            }
        }
    }
}
