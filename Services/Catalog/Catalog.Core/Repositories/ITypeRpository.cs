using Catalog.Core.Entities;

namespace Catalog.Core.Repositories;

public interface ITypeRpository
{
    Task<IEnumerable<ProductType>> GetAllTypesAsync();
}
