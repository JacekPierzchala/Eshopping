using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application;
public class GetAllBrandsQuery:IRequest<IList<BrandResponse>>
{

}
