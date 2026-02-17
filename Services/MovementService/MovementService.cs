using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using controle_estoque_cshap.DTOs.MovementDto;
using controle_estoque_cshap.Repositories.MovementRepository;

namespace controle_estoque_cshap.Services.MovementService;

public class MovementService : IMovementService
{
    private readonly IMovementRepository _movementRepository;

    public MovementService(IMovementRepository movementRepository)
    {
        _movementRepository = movementRepository;
    }

    public async Task<List<MovementDto>> GetByItemAsync(int itemId)
    {
        var movements = await _movementRepository.GetByItemAsync(itemId);

        return movements.Select(m => new MovementDto
        {
            MovementId = m.MovementId,
            Date = m.Date!.Value,
            Type = m.Type,
            QuantityMoved = m.QuantityMoved,
            PreviousQuantity = m.PreviousQuantity,
            NewQuantity = m.NewQuantity,
            ItemId = m.ItemId,
            ProductName = m.Item.Product.Name,
            UserId = m.UserId,
            UserName = m.User.Name
        }).ToList();
    }

    public async Task<List<MovementDto>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        var movements = await _movementRepository.GetByPeriodAsync(startDate, endDate);

        return movements.Select(m => new MovementDto
        {
            MovementId = m.MovementId,
            Date = m.Date!.Value,
            Type = m.Type,
            QuantityMoved = m.QuantityMoved,
            PreviousQuantity = m.PreviousQuantity,
            NewQuantity = m.NewQuantity,
            ItemId = m.ItemId,
            ProductName = m.Item.Product.Name,
            UserId = m.UserId,
            UserName = m.User.Name
        }).ToList();
    }
}
