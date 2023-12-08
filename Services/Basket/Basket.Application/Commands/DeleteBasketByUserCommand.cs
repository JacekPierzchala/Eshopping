using MediatR;

namespace Basket.Application.Commands;
public class DeleteBasketByUserCommand:IRequest
{
    public string UserName { get; set; }
}
