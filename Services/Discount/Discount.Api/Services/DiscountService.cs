using Discount.Application.Commands;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.Api.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DiscountService> _logger;

    public DiscountService(IMediator mediator, ILogger<DiscountService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetDiscountQuery { ProductName = request.ProductName });
        _logger.LogInformation($"discount is retrieved for {request.ProductName} for amount {result.Amount}");
        return result;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new CreateDiscountCommand
        {
            ProductName = request.Coupon.ProductName,
            Description = request.Coupon.Description,
            Amount = request.Coupon.Amount,
        });
        _logger.LogInformation($"Discount is created for {request.Coupon.ProductName} for amount {request.Coupon.Amount}");
        return result;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new UpdateDiscountCommand
        {
            Id=request.Coupon.Id,
            ProductName = request.Coupon.ProductName,
            Description = request.Coupon.Description,
            Amount = request.Coupon.Amount,
        });
        _logger.LogInformation($"Discount is updated for {request.Coupon.ProductName} for amount {request.Coupon.Amount}");
        return result;
    }
    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send
            (new DeleteDiscountCommand { ProductName = request.ProductName });
        var response= new DeleteDiscountResponse { Success= result };

        return response;

    }
}
