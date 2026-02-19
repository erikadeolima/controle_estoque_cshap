using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using controle_estoque_cshap.DTOs.ItemDto;
using controle_estoque_cshap.Services.ItemService;

namespace controle_estoque_cshap.Controllers;

[ApiController]
[Route("api/items")]
public class ItemController : ControllerBase
{
  private readonly IItemService _itemService;
  private readonly ILogger<ItemController> _logger;

  public ItemController(IItemService itemService, ILogger<ItemController> logger)
  {
    _itemService = itemService;
    _logger = logger;
  }

  /// <summary>
  /// Returns the list of items.
  /// </summary>
  [HttpGet]
  [ProducesResponseType(typeof(IEnumerable<ItemDto>), 200)]
  [ProducesResponseType(500)]
  public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll()
  {
    try
    {
      var items = await _itemService.GetAllAsync();
      return Ok(items);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erro ao obter itens.");
      return StatusCode(500, new { message = "Erro ao obter itens." });
    }
  }

  /// <summary>
  /// Returns an item by id.
  /// </summary>
  [HttpGet("{id:int}")]
  [ProducesResponseType(typeof(ItemDto), 200)]
  [ProducesResponseType(404)]
  [ProducesResponseType(500)]
  public async Task<ActionResult<ItemDto>> GetById(int id)
  {
    try
    {
      var item = await _itemService.GetItemByIdAsync(id);
      if (item == null)
        return NotFound(new { message = "Item nao encontrado." });

      return Ok(item);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erro ao obter item.");
      return StatusCode(500, new { message = "Erro ao obter item." });
    }
  }

  /// <summary>
  /// Returns items by product id.
  /// </summary>
  [HttpGet("/api/products/{productId:int}/items")]
  [ProducesResponseType(typeof(IEnumerable<ItemDto>), 200)]
  [ProducesResponseType(500)]
  public async Task<ActionResult<IEnumerable<ItemDto>>> GetByProduct(int productId)
  {
    try
    {
      var items = await _itemService.GetByProductIdAsync(productId);
      return Ok(items);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erro ao obter itens do produto.");
      return StatusCode(500, new { message = "Erro ao obter itens do produto." });
    }
  }

  /// <summary>
  /// Returns items that expire within the given number of days.
  /// </summary>
  [HttpGet("expiring")]
  [ProducesResponseType(typeof(IEnumerable<ItemDto>), 200)]
  [ProducesResponseType(400)]
  [ProducesResponseType(500)]
  public async Task<ActionResult<IEnumerable<ItemDto>>> GetExpiring([FromQuery] int days = 7)
  {
    if (days < 0)
      return BadRequest(new { message = "days deve ser maior ou igual a zero." });

    try
    {
      var items = await _itemService.GetExpiringItemsAsync(days);
      return Ok(items);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erro ao obter itens com vencimento proximo.");
      return StatusCode(500, new { message = "Erro ao obter itens com vencimento proximo." });
    }
  }

  /// <summary>
  /// Creates a new item.
  /// </summary>
  [HttpPost("/api/products/{productId:int}/items")]
  [ProducesResponseType(typeof(ItemDto), 201)]
  [ProducesResponseType(400)]
  [ProducesResponseType(404)]
  [ProducesResponseType(500)]
  public async Task<ActionResult<ItemDto>> Create(int productId, [FromBody] ItemCreateDto dto)
  {
    if (productId <= 0)
      return BadRequest(new { message = "ProductId deve ser maior que zero." });

    dto.ProductId = productId;

    try
    {
      var created = await _itemService.CreateItemAsync(dto);
      if (created == null)
        return NotFound(new { message = "Produto nao encontrado." });

      return CreatedAtAction(nameof(GetById), new { id = created.ItemId }, created);
    }
    catch (ArgumentException ex)
    {
      _logger.LogWarning(ex, "Dados invalidos ao criar item.");
      return BadRequest(new { message = ex.Message });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erro ao criar item.");
      return StatusCode(500, new { message = "Erro ao criar item." });
    }
  }

  /// <summary>
  /// Updates an existing item.
  /// </summary>
  [HttpPut("{id:int}")]
  [ProducesResponseType(204)]
  [ProducesResponseType(400)]
  [ProducesResponseType(404)]
  [ProducesResponseType(500)]
  public async Task<IActionResult> Update(int id, [FromBody] ItemUpdateDto dto)
  {
    try
    {
      var updated = await _itemService.UpdateItemAsync(id, dto);
      if (updated == null)
        return NotFound(new { message = "Item nao encontrado." });

      return NoContent();
    }
    catch (InvalidOperationException ex)
    {
      _logger.LogWarning(ex, "Tentativa de atualizar item inativo.");
      return Conflict(new { message = ex.Message });
    }
    catch (ArgumentException ex)
    {
      _logger.LogWarning(ex, "Dados invalidos ao atualizar item.");
      return BadRequest(new { message = ex.Message });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erro ao atualizar item.");
      return StatusCode(500, new { message = "Erro ao atualizar item." });
    }
  }

  /// <summary>
  /// Inactivates an item by id.
  /// </summary>
  [HttpDelete("{id:int}")]
  [ProducesResponseType(204)]
  [ProducesResponseType(404)]
  [ProducesResponseType(500)]
  public async Task<IActionResult> Inactivate(int id)
  {
    try
    {
      var result = await _itemService.DeleteItemAsync(id);
      if (result == ItemDeleteResult.NotFound)
        return NotFound(new { message = "Item nao encontrado." });

      if (result == ItemDeleteResult.AlreadyInactive)
        return Conflict(new { message = "Item inativo nao pode ser removido manualmente." });

      return NoContent();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erro ao excluir item.");
      return StatusCode(500, new { message = "Erro ao excluir item." });
    }
  }

  /// <summary>
  /// Generates a CSV report of items expiring within the specified number of days.
  /// Uses JOIN across Item, Product, and Category tables.
  /// </summary>
  [HttpGet("reports/expiration")]
  [ProducesResponseType(typeof(FileResult), 200)]
  [ProducesResponseType(404)]
  [ProducesResponseType(500)]
  public async Task<IActionResult> GetExpirationReport([FromQuery] int days = 7)
  {
    if (days < 1)
      return BadRequest(new { message = "days deve ser maior ou igual a 1." });

    try
    {
      var csv = await _itemService.GenerateExpirationReportCsvAsync(days);

      if (string.IsNullOrEmpty(csv))
        return NotFound(new { message = "Nenhum item proximo a vencer encontrado." });

      var fileName = $"relatorio_expiracao_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
      var bytes = System.Text.Encoding.UTF8.GetBytes(csv);

      return File(bytes, "text/csv", fileName);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erro ao gerar relatorio de expiracao.");
      return StatusCode(500, new { message = "Erro ao gerar relatorio de expiracao." });
    }
  }

  /// <summary>
  /// Generates a CSV report of items that have already expired.
  /// Uses JOIN across Item, Product, and Category tables.
  /// </summary>
  [HttpGet("reports/expired")]
  [ProducesResponseType(typeof(FileResult), 200)]
  [ProducesResponseType(404)]
  [ProducesResponseType(500)]
  public async Task<IActionResult> GetExpiredItemsReport()
  {
    try
    {
      var csv = await _itemService.GenerateExpiredItemsReportCsvAsync();

      if (string.IsNullOrEmpty(csv))
        return NotFound(new { message = "Nenhum item vencido encontrado." });

      var fileName = $"relatorio_vencidos_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
      var bytes = System.Text.Encoding.UTF8.GetBytes(csv);

      return File(bytes, "text/csv", fileName);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erro ao gerar relatorio de itens vencidos.");
      return StatusCode(500, new { message = "Erro ao gerar relatorio de itens vencidos." });
    }
  }
}
