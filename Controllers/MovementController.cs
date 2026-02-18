using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using controle_estoque_cshap.Services.MovementService;

namespace controle_estoque_cshap.Controllers;

[ApiController]
[Route("api")]
public class MovementController : ControllerBase
{
    private readonly IMovementService _movementService;

    public MovementController(IMovementService movementService)
    {
        _movementService = movementService;
    }

    // GET /api/items/{itemId}/movements
    [HttpGet("items/{itemId}/movements")]
    public async Task<IActionResult> GetByItem(int itemId)
    {
        var result = await _movementService.GetByItemAsync(itemId);

        if (result == null || result.Count == 0)
            return NotFound();

        return Ok(result);
    }

    // GET /api/movements?startDate=X&endDate=Y
    [HttpGet("movements")]
    public async Task<IActionResult> GetByPeriod(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var result = await _movementService.GetByPeriodAsync(startDate, endDate);

        if (result == null || result.Count == 0)
            return NotFound();

        return Ok(result);
    }
}
