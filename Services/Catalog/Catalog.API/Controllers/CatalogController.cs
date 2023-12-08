using Catalog.Application;
using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers;

public class CatalogController : ApiController
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("[action]/{id}",Name = "GetProductById")]
    [ProducesResponseType(typeof(ProductResponse),(int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductResponse>>GetProductById(string id)
    {
        var query = new GetProductByIdQuery { Id= id };
        var result=await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("[action]/{productName}", Name = "GetProductByProductName")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetProductByProductName(string productName)
    {
        var query = new GetProductByNameQuery {  Name = productName };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("GetAllProducts")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetAllProducts([FromQuery] CatalogSpecsParams catalogSpecsParams)
    {
        var query = new GetAllProductsQuery { CatalogSpecsParams= catalogSpecsParams };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("GetAllBrands")]
    [ProducesResponseType(typeof(IList<BrandResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<BrandResponse>>> GetAllBrands()
    {
        var query = new GetAllBrandsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("GetAllTypes")]
    [ProducesResponseType(typeof(IList<TypesResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<TypesResponse>>> GetAllTypes()
    {
        var query = new GetAllTypesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("[action]/{brandName}", Name = "GetProductsByBrandName")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetProductsByBrandName(string brandName)
    {
        var query = new GetProductByBrandQuery {  BrandName = brandName };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("CreateProduct")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("UpdateProduct")]
    [ProducesResponseType(typeof(bool),(int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool),(int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        var result = await _mediator.Send(new DeleteProductCommand{ ProductId=id });
        return Ok(result);
    }
}
