﻿using MediatR;

namespace Catalog.Application.Commands;
public class DeleteProductCommand:IRequest<bool> 
{
    public string ProductId { get; set; }
}