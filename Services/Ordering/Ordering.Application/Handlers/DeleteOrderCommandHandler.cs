﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;
public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository,
        ILogger<DeleteOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }
    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToDelete= await _orderRepository.GetByIdAsync(request.OrderId);
        if (orderToDelete == null) 
        {
            throw new OrderNotFoundException(nameof(Order),request.OrderId);
        }
        await _orderRepository.DeleteAsync(orderToDelete);
        _logger.LogInformation($"Order with id {request.OrderId} deleted");
        return Unit.Value;  
    }
}
