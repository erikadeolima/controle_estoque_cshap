using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using controle_estoque_cshap.DTOs.MovementDto;
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

    /// <summary>
    /// Movements by item
    /// </summary>
    [HttpGet("items/{itemId:int}/movements")]
    [ProducesResponseType(typeof(IEnumerable<MovementDto>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<MovementDto>>> GetByItem(int itemId)
    {
        try
        {
            var movements = await _movementService.GetByItemAsync(itemId);

            if (movements == null || movements.Count == 0)
                return NotFound(new { message = "Nenhuma movimentação encontrada para este item." });

            return Ok(movements);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Erro ao buscar movimentações por item.",
                detail = ex.Message
            });
        }
    }

    /// <summary>
    /// Movements by period
    /// </summary>
    [HttpGet("movements")]
    [ProducesResponseType(typeof(IEnumerable<MovementDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<MovementDto>>> GetByPeriod(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            if (startDate == default || endDate == default)
                return BadRequest(new
                {
                    message = "Parâmetros startDate e endDate são obrigatórios."
                });

            var movements = await _movementService.GetByPeriodAsync(startDate, endDate);

            if (movements == null || movements.Count == 0)
                return NotFound(new { message = "Nenhuma movimentação encontrada no período informado." });

            return Ok(movements);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Erro ao buscar movimentações por período.",
                detail = ex.Message
            });
        }
    }
}
