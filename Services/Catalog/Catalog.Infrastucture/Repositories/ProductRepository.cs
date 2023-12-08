using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastucture.Repositories;
public class ProductRepository : IProductRepository, ITypeRpository, IBrandRepository
{
    private readonly ICatalogContext _catalogContext;

    public ProductRepository(ICatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }
    public async Task<Product> CreateProduct(Product product)
    {
        await _catalogContext.Products.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> DeleteProduct(string productId)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, productId);
        var result = await _catalogContext.Products.DeleteOneAsync(filter);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<Pagination<Product>> GetAllAsync(CatalogSpecsParams catalogSpecsParams)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;
        if (!string.IsNullOrEmpty(catalogSpecsParams.Search))
        {
            var search = builder.Regex(x => x.Name, new BsonRegularExpression(catalogSpecsParams.Search));
            filter &= search;
        }
        if (!string.IsNullOrEmpty(catalogSpecsParams.BrandId))
        {
            var search = builder.Eq(x => x.Brands.Id, catalogSpecsParams.BrandId);
            filter &= search;
        }
        if (!string.IsNullOrEmpty(catalogSpecsParams.TypeId))
        {
            var search = builder.Eq(x => x.Types.Id, catalogSpecsParams.TypeId);
            filter &= search;
        }
        if (!string.IsNullOrEmpty(catalogSpecsParams.Sort))
        {
            return new Pagination<Product>
            {
                PageSize = catalogSpecsParams.PageSize,
                PageIndex = catalogSpecsParams.PageIndex,
                Data=await DataFilter(catalogSpecsParams,filter),
                Count = await _catalogContext.Products.CountDocumentsAsync(p => true)
            };
        }
        return new Pagination<Product>
        {
            PageSize = catalogSpecsParams.PageSize,
            PageIndex = catalogSpecsParams.PageIndex,
            Data = await _catalogContext.Products.Find(filter)
             .Sort(Builders<Product>.Sort.Ascending("Name"))
             .Skip(catalogSpecsParams.PageSize * (catalogSpecsParams.PageIndex - 1))
             .Limit(catalogSpecsParams.PageSize)
             .ToListAsync(),
            Count = await _catalogContext.Products.CountDocumentsAsync(p => true)
        };
    }

    private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecsParams catalogSpecParams, FilterDefinition<Product> filter)
    {
        //var result = catalogSpecParams.Sort switch
        //{
        //    "priceAsc" => await _catalogContext.Products.Find(filter)
        //     .Sort(Builders<Product>.Sort.Ascending("Price"))
        //     .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
        //     .Limit(catalogSpecParams.PageSize)
        //     .ToListAsync(),

        //    "priceDesc" => await _catalogContext.Products.Find(filter)
        //    .Sort(Builders<Product>.Sort.Descending("Price"))
        //    .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
        //    .Limit(catalogSpecParams.PageSize)
        //    .ToListAsync(),
        //    _ => await _catalogContext.Products.Find(filter)
        //   .Sort(Builders<Product>.Sort.Ascending("Name"))
        //   .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
        //   .Limit(catalogSpecParams.PageSize)
        //   .ToListAsync()
        //};

        //return result;
        switch (catalogSpecParams.Sort)
        {
            case "priceAsc":
                return await _catalogContext
                    .Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Ascending("Price"))
                    .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                    .Limit(catalogSpecParams.PageSize)
                    .ToListAsync();
            case "priceDesc":
                return await _catalogContext
                    .Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Descending("Price"))
                    .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                    .Limit(catalogSpecParams.PageSize)
                    .ToListAsync();
            default:
                return await _catalogContext
                    .Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Ascending("Name"))
                    .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                    .Limit(catalogSpecParams.PageSize)
                    .ToListAsync();
        }
    }

    public async Task<IEnumerable<ProductType>> GetAllTypesAsync()
    {
        return await _catalogContext
        .Types
        .Find(p => true).ToListAsync();
    }

    public async Task<IEnumerable<ProductBrand>> GetBrandsAsync()
    {
        return await _catalogContext
        .Brands
        .Find(p => true).ToListAsync();
    }

    public async Task<Product> GetProduct(string productId)
    {
        return await _catalogContext
        .Products
        .Find(p => p.Id == productId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByBrand(string brandName)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Brands.Name, brandName);
        return await _catalogContext
        .Products
        .Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByName(string productName)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Name, productName);
        return await _catalogContext
        .Products
        .Find(filter).ToListAsync();
    }

    public async Task<bool> UpdateProducts(Product product)
    {
        var result = await _catalogContext
                .Products.ReplaceOneAsync(p => p.Id == product.Id, product);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }
}
