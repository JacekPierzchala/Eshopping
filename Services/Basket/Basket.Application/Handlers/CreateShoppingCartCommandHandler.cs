using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;
public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
{
    private readonly IBasketRepository _basketRepository;

    public CreateShoppingCartCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }
    public async Task<ShoppingCartResponse> 
        Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        //todo discount service adn applu coupons

        var shoppingCart = await _basketRepository.UpdateBasket(new ShoppingCart 
        { 
             UserName = request.UserName,
             Items = BasketMapper.MapperExt.Map<List<ShoppingCartItem>>( request.Items)
        });
        var shopppingCartResponse= BasketMapper.MapperExt.Map<ShoppingCartResponse>(shoppingCart);
        return shopppingCartResponse;
    }
}
