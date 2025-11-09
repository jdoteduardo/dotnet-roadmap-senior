using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Week01_EFCore.DTOs;
using Week01_EFCore.Features.Products.Commands.CreateProduct;
using Week01_EFCore.Features.Products.Commands.DeleteProduct;
using Week01_EFCore.Features.Products.Commands.UpdateProduct;
using Week01_EFCore.Features.Products.Queries.GetProductById;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMediator _mediator;

        public ProductController(IProductService productService, IMediator mediator)
        {
            _productService = productService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO?>> GetById(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });

            if (product == null)
            {
                return NotFound(new { Message = "Product not found." });
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Create(CreateProductCommand command)
        {
            var createdProduct = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDTO>> Update(int id, UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { Message = "The URL ID does not match the command ID." });
            }

            var updatedProduct = await _mediator.Send(command);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _mediator.Send(new DeleteProductCommand { Id = id });

            if (!deleted)
            {
                return NotFound(new { Message = "Product not found." });
            }

            return NoContent();
        }
    }
}
