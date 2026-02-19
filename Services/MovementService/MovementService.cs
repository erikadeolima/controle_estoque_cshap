using controle_estoque_cshap.DTOs.MovementDto;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.ItemRepository;
using controle_estoque_cshap.Repositories.MovementRepository;

namespace controle_estoque_cshap.Services.MovementService;

public class MovementService : IMovementService
{
    private readonly IMovementRepository _movementRepository;
    private readonly IItemRepository _itemRepository;

    public MovementService(
        IMovementRepository movementRepository,
        IItemRepository itemRepository)
    {
        _movementRepository = movementRepository;
        _itemRepository = itemRepository;
    }

    public async Task<List<MovementDto>> GetByItemAsync(int itemId)
    {
        var movements = await _movementRepository.GetByItemAsync(itemId);

        return movements.Select(m => Map(m)).ToList();
    }

    public async Task<List<MovementDto>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        var movements = await _movementRepository.GetByPeriodAsync(startDate, endDate);

        return movements.Select(m => Map(m)).ToList();
    }

    public async Task<MovementDto> CreateAsync(CreateMovementDto dto)
    {
        var item = await _itemRepository.GetByIdAsync(dto.ItemId);

        if (item == null)
            throw new Exception("Item não encontrado.");

        var previousQuantity = item.Quantity;
        var newQuantity = previousQuantity;

        if (dto.Type.ToUpper() == "IN")
            newQuantity += dto.Quantity;
        else if (dto.Type.ToUpper() == "OUT")
            newQuantity -= dto.Quantity;
        else
            throw new Exception("Tipo de movimentação inválido.");

        if (newQuantity < 0)
            throw new Exception("Estoque insuficiente.");

        item.Quantity = newQuantity;

        await _itemRepository.UpdateAsync(item);

        var movement = new Movement
        {
            ItemId = dto.ItemId,
            UserId = dto.UserId,
            Date = DateTime.UtcNow,
            Type = dto.Type,
            QuantityMoved = dto.Quantity,
            PreviousQuantity = previousQuantity,
            NewQuantity = newQuantity
        };

        var created = await _movementRepository.CreateAsync(movement);

        return Map(created);
    }

    private static MovementDto Map(Movement m)
    {
        return new MovementDto
        {
            MovementId = m.MovementId,
            Date = m.Date!.Value,
            Type = m.Type,
            QuantityMoved = m.QuantityMoved,
            PreviousQuantity = m.PreviousQuantity,
            NewQuantity = m.NewQuantity,
            ItemId = m.ItemId,
            ProductName = m.Item?.Product?.Name ?? "",
            UserId = m.UserId,
            UserName = m.User?.Name ?? ""
        };
    }
}
