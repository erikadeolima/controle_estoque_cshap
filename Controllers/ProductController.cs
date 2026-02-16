using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using controle_estoque_cshap.DTOs.ProductDto;
using controle_estoque_cshap.Services.ProductService;
using controle_estoque_cshap.Services.ItemService;

namespace controle_estoque_cshap.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
  private readonly IProductService _productService;
  private readonly IItemService _itemService;

  public ProductController(IProductService productService, IItemService itemService)
  {
    _productService = productService;
    _itemService = itemService;
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
  /// Returns the products actives.
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
  /// <summary>
  /// Returns a product by id.
  /// </summary>
  [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<ProductActiveDto>), 200)]
    [ProducesResponseType(500)]
    public ActionResult<IEnumerable<ProductActiveDto>> GetActiveProducts()
    {
        try
        {
            var products = _productService.GetActiveProducts();
            if (products == null || products.Count == 0)
            {
                return NotFound(new { message = "Nenhum produto ativo encontrado." });
            }

            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao obter produtos ativos.", detail = ex.Message });
        }
    }
  /// <summary>
  /// Returns a sku.
  /// </summary>
  [HttpGet("by-sku/{sku}")]
  [ProducesResponseType(typeof(ProductDto), 200)]
  [ProducesResponseType(404)]
  [ProducesResponseType(500)]
  public async Task<ActionResult<ProductDto>> GetBySkuAsync(string sku)
  {
    try
    {
      var product = await _productService.GetBySkuAsync(sku);
      
      if (product == null || product.Status == 0)
      {
        return NotFound(new { message = "Produto não encontrado"});
      }
      return Ok(product);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new
      {
        message = "Erro ao buscar produto por SKU.",
        detail = ex.Message
      });
    }
  }
  [HttpPost]
  [ProducesResponseType(typeof(ProductDto), 201)]
  [ProducesResponseType(400)]
  [ProducesResponseType(500)]
public async Task<ActionResult<ProductDto>> Create([FromBody] ProductCreateDto dto)
{
    try
    {
        var createdProduct = await _productService.CreateAsync(dto);

        if (createdProduct == null)
            return BadRequest(new { message = "Não foi possível criar o produto" });

        if (dto.Items != null && dto.Items.Count > 0)
        {
            foreach (var item in dto.Items)
            {
                item.ProductId = createdProduct.ProductId;
                await _itemService.CreateItemAsync(item);
            }
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProduct.ProductId },
            createdProduct
        );
    }
    catch (Exception ex)
    {
        return StatusCode(500, new
        {
            message = "Erro ao criar o produto.",
            detail = ex.Message
        });
    }
  }
   /// <summary>
  /// Returns a sku.
  /// </summary>
[HttpPut("{id:int}")]
[ProducesResponseType(typeof(ProductDto), 200)]
[ProducesResponseType(404)]
[ProducesResponseType(500)]
public async Task<ActionResult<ProductDto>> Update(
    int id,
    [FromBody] ProductUpdateDto dto)
{
    try
    {
        var product = await _productService.UpdateAsync(id, dto);

        if (product == null)
            return NotFound(new { message = "Produto não encontrado." });

        return Ok(product);
    }
    catch (Exception ex)
    {
        return StatusCode(500, new
        {
            message = "Erro ao atualizar produto.",
            detail = ex.Message
        });
    }
}
  /// <summary>
  /// Returns delete.
  /// </summary>
[HttpDelete("{id:int}")]
[ProducesResponseType(204)]
[ProducesResponseType(404)]
[ProducesResponseType(500)]
public async Task<IActionResult> Delete(int id)
{
    try
    {
        var deleted = await _productService.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "Produto não encontrado." });

        return NoContent();
    }
    catch (Exception ex)
    {
        return StatusCode(500, new
        {
            message = "Erro ao excluir produto.",
            detail = ex.Message
        });
    }
}




}
