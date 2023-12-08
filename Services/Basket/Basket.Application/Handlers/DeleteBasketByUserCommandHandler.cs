using Basket.Application.Commands;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;
public class DeleteBasketByUserCommandHandler : IRequestHandler<DeleteBasketByUserCommand>
{
    private readonly IBasketRepository _basketRepository;

    public DeleteBasketByUserCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }
    public async Task<Unit> Handle(DeleteBasketByUserCommand request, CancellationToken cancellationToken)
    {
        await _basketRepository.DeleteBasket(request.UserName);
        return Unit.Value;
    }
}
