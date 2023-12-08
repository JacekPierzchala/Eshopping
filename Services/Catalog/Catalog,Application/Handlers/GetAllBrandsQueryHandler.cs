using AutoMapper;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;
public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandResponse>>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;

    public GetAllBrandsQueryHandler(IBrandRepository brandRepository, IMapper mapper)
    {
        _brandRepository = brandRepository;
        _mapper = mapper;
    }
    public async Task<IList<BrandResponse>> 
        Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        var brandList = await _brandRepository.GetBrandsAsync();
        var brandResponseList= ProducMapper.MapperExt.Map<IList<ProductBrand>, IList<BrandResponse>>
                             (brandList.ToList());
        return brandResponseList;
    }
}
