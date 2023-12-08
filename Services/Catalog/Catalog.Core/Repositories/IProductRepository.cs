using Catalog.Core.Entities;
using Catalog.Core.Specs;

namespace Catalog.Core.Repositories;

public interface IProductRepository
{
    Task<Pagination<Product>> GetAllAsync(CatalogSpecsParams catalogSpecsParams);
    Task<Product>GetProduct(string productId);
    Task<IEnumerable<Product>> GetProductsByName(string productName);
    Task<IEnumerable<Product>> GetProductsByBrand(string brandName);
    Task<Product>CreateProduct(Product product);
    Task<bool>UpdateProducts(Product product);  
    Task<bool>DeleteProduct(string productId);  
}
