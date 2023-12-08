using Basket.Api.Controllers;
using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

public class BasketController : APIController
{
    private readonly IMediator _mediator;
    private readonly DiscountGrpcService _discountGrpcService;
    private readonly IPublishEndpoint _publishEndpoint;

    public BasketController(IMediator mediator, DiscountGrpcService discountGrpcService,
        IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _discountGrpcService = discountGrpcService;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    [Route("[action]/{userName}", Name = "GetBasketByUserName")]
    [ProducesResponseType(typeof(ShoppingCartResponse),(int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>>GetBasket(string userName)
    {
        var query = new GetBasketByUserNameQuery { UserName=userName };
        var basket= await _mediator.Send(query);
        return Ok(basket);
    }

    [HttpPost("CreateBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket([FromBody] CreateShoppingCartCommand createShoppingCartCommand)
    {
        foreach (var item in createShoppingCartCommand.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }
        var basket = await _mediator.Send(createShoppingCartCommand);
        return Ok(basket);
    }

    [HttpDelete]
    [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> DeleteBasket(string userName)
    {
        var query = new DeleteBasketByUserCommand { UserName = userName };
        return Ok( await _mediator.Send(query));

    }

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        var query=  new GetBasketByUserNameQuery { UserName = basketCheckout.UserName };
        var basket= await _mediator.Send(query);
        if(basket==null)
        {
            return BadRequest();
        }
        var eventMessage = BasketMapper.MapperExt.Map<BasketCheckoutEvent>(basketCheckout);
        eventMessage.TotalPrice= basketCheckout.TotalPrice; 
        await _publishEndpoint.Publish(eventMessage);

        var deleteQuery= new DeleteBasketByUserCommand { UserName = basketCheckout.UserName };
        await _mediator.Send(deleteQuery);
        return Accepted();
    }
}
