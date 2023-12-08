using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers;
internal class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, IList<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetProductByNameQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<IList<ProductResponse>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProductsByName(request.Name);
        var productsResponse=ProducMapper.MapperExt.Map<IList<ProductResponse>>(products);
        return productsResponse;
    }
}
