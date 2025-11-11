using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Week01_EFCore.DTOs;
using Week01_EFCore.Features.Categories.Commands.CreateCategory;
using Week01_EFCore.Features.Categories.Commands.DeleteCategory;
using Week01_EFCore.Features.Categories.Commands.UpdateCategory;
using Week01_EFCore.Features.Categories.Queries;
using Week01_EFCore.Interfaces;
using Week01_EFCore.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Week01_EFCore.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMediator _mediator;

        public CategoryController(ICategoryService categoryService, IMediator mediator)
        {
            _categoryService = categoryService;
            _mediator = mediator;
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
