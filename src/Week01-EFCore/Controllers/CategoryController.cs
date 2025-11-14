using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Features.Categories.Commands.CreateCategory;
using ECommerce.OrderManagement.Application.Features.Categories.Commands.DeleteCategory;
using ECommerce.OrderManagement.Application.Features.Categories.Commands.UpdateCategory;
using ECommerce.OrderManagement.Application.Features.Categories.Queries.GetCategoryById;
using ECommerce.OrderManagement.Application.Features.Categories.Queries.GetAllCategories;
using ECommerce.OrderManagement.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.OrderManagement.API.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICategoryService _categoryService;

        public CategoryController(IMediator mediator, ICategoryService categoryService)
        {
            _mediator = mediator;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO?>> GetById(int id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery { Id = id });

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Create(CreateCategoryCommand command)
        {
            var createdCategory = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDTO>> Update(int id, UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { Message = "The URL ID does not match the command ID." });
            }

            var updatedCategory = await _mediator.Send(command);
            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _mediator.Send(new DeleteCategoryCommand { Id = id });

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
