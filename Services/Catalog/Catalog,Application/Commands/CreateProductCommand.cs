﻿using Catalog.Application.Responses;

using MediatR;

namespace Catalog.Application.Commands;
public class CreateProductCommand:IRequest<ProductResponse>
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string ImageFile { get; set; }
    public decimal Price { get; set; }
    public BrandResponse Brands { get; set; }
    public TypesResponse Types { get; set; }
}
