using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using controle_estoque_cshap.DTOs.CategoryDto;
using controle_estoque_cshap.Services.CategoryService;

namespace controle_estoque_cshap.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
  private readonly ICategoryService _categoryService;

  public CategoryController(ICategoryService categoryService)
  {
    _categoryService = categoryService;
  }

  /// <summary>
  /// Returns the list of categories.
  /// </summary>
  [HttpGet]
  [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
  [ProducesResponseType(500)]
  public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
  {
    try
    {
      var categories = await _categoryService.GetAllCategoriesAsync();
      return Ok(categories);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { message = "Erro ao obter categorias." });
    }
  }

  /// <summary>
  /// Returns a category by id.
  /// </summary>
  [HttpGet("{id:int}")]
  [ProducesResponseType(typeof(CategoryDto), 200)]
  [ProducesResponseType(404)]
  [ProducesResponseType(500)]
  public async Task<ActionResult<CategoryDto>> GetById(int id)
  {
    try
    {
      var category = await _categoryService.GetCategoryByIdAsync(id);

      if (category == null)
        return NotFound(new { message = "Categoria nao encontrada." });

      return Ok(category);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { message = "Erro ao obter categoria." });
    }
  }

  /// <summary>
  /// Creates a new category.
  /// </summary>
  [HttpPost]
  [ProducesResponseType(typeof(CategoryDto), 201)]
  [ProducesResponseType(400)]
  [ProducesResponseType(409)]
  [ProducesResponseType(500)]
  public async Task<ActionResult<CategoryDto>> Create([FromBody] CategoryCreateDto dto)
  {
    try
    {
      var created = await _categoryService.CreateCategoryAsync(dto);
      if (created == null)
        return Conflict(new { message = "Categoria com este nome ja existe." });

      return CreatedAtAction(nameof(GetById), new { id = created.CategoryId }, created);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { message = "Erro ao criar categoria." });
    }
  }

  /// <summary>
  /// Updates an existing category.
  /// </summary>
  [HttpPut("{id:int}")]
  [ProducesResponseType(204)]
  [ProducesResponseType(400)]
  [ProducesResponseType(404)]
  [ProducesResponseType(409)]
  [ProducesResponseType(500)]
  public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
  {
    try
    {
      var updated = await _categoryService.UpdateCategoryAsync(id, dto);
      if (updated == CategoryUpdateResult.NotFound)
        return NotFound(new { message = "Categoria nao encontrada." });

      if (updated == CategoryUpdateResult.Conflict)
        return Conflict(new { message = "Categoria com este nome ja existe." });

      return NoContent();
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { message = "Erro ao atualizar categoria." });
    }
  }
}
