using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using controle_estoque_cshap.DTOs;
using controle_estoque_cshap.Services;

namespace controle_estoque_cshap.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
  private readonly IProductService _productService;

  public ProductController(IProductService productService)
  {
    _productService = productService;
  }

  /// <summary>
  /// Returns the list of inactive products.
  /// </summary>
  [HttpGet("inactive")]
  [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
  [ProducesResponseType(500)]
  public async Task<ActionResult<IEnumerable<ProductDto>>> GetInactive()
  {
    try
    {
      var products = await _productService.GetInactiveAsync();
      if (products == null)
      {
        return NotFound(new { message = "Nenhum produto inativo encontrado." });
      }

      return Ok(products);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { message = "Erro ao obter produtos inativos.", detail = ex.Message });
    }
  }

  /// <summary>
  /// Returns a product by id.
  /// </summary>
  [HttpGet("{id:int}")]
  [ProducesResponseType(typeof(ProductDto), 200)]
  [ProducesResponseType(404)]
  [ProducesResponseType(500)]
  public async Task<ActionResult<ProductDto>> GetById(int id)
  {
    try
    {
      var product = await _productService.GetByIdAsync(id);

      if (product == null || product.Status == 0)
      {
        return NotFound(new { message = "Produto nao encontrado." });
      }

      return Ok(product);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { message = "Erro ao obter produto.", detail = ex.Message });
    }
  }
}
