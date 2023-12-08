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
internal class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, IList<TypesResponse>>
{
    private readonly ITypeRpository _typeRpository;

    public GetAllTypesQueryHandler(ITypeRpository typeRpository)
    {
        _typeRpository = typeRpository;
    }
    public async Task<IList<TypesResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await _typeRpository.GetAllTypesAsync();
        var typesResponse=ProducMapper.MapperExt.Map<IList<TypesResponse>>(types);
        return typesResponse;
    }
}
