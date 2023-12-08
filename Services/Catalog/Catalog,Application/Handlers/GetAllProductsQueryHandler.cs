using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;

namespace Catalog.Application.Handlers;
internal class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var productList = await _productRepository.GetAllAsync(request.CatalogSpecsParams);
        var productResponseList = ProducMapper.MapperExt.Map<Pagination<ProductResponse>>(productList);
        return productResponseList;
    }
}
