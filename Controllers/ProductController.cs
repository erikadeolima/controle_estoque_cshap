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
      return Ok(products);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { message = "Erro ao obter produtos inativos.", detail = ex.Message });
    }
  }
}
