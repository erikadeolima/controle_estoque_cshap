using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using controle_estoque_cshap.DTOs.ItemDto;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.ItemRepository;
using controle_estoque_cshap.Repositories.ProductRepository;

namespace controle_estoque_cshap.Services.ItemService;

public class ItemService : IItemService
{
  private readonly IItemRepository _itemRepository;
  private readonly IProductRepository _productRepository;

  public ItemService(IItemRepository itemRepository, IProductRepository productRepository)
  {
    _itemRepository = itemRepository;
    _productRepository = productRepository;
  }

  public async Task<IEnumerable<ItemDto>> GetAllAsync()
  {
    var items = await _itemRepository.GetAllAsync();
    return items.Select(ItemDto.FromModel);
  }

  public async Task<ItemDto?> GetItemByIdAsync(int id)
  {
    var item = await _itemRepository.GetByIdAsync(id);
    return item == null ? null : ItemDto.FromModel(item);
  }

  public async Task<IEnumerable<ItemDto>> GetByProductIdAsync(int productId)
  {
    var items = await _itemRepository.GetByProductIdAsync(productId);
    return items.Select(ItemDto.FromModel);
  }

  public async Task<IEnumerable<ItemDto>> GetExpiringItemsAsync(int days)
  {
    var items = await _itemRepository.GetExpiringItemsAsync(days);
    return items.Select(ItemDto.FromModel);
  }

  public async Task<ItemDto?> CreateItemAsync(ItemCreateDto dto)
  {
    if (string.IsNullOrWhiteSpace(dto.Batch))
      throw new System.ArgumentException("Batch e obrigatorio.", nameof(dto.Batch));

    if (dto.Quantity < 0)
      throw new System.ArgumentException("Quantity deve ser maior ou igual a zero.", nameof(dto.Quantity));

    if (string.IsNullOrWhiteSpace(dto.Location))
      throw new System.ArgumentException("Location e obrigatoria.", nameof(dto.Location));

    if (dto.ExpirationDate.HasValue && dto.ExpirationDate.Value <= System.DateTime.Now)
      throw new System.ArgumentException("ExpirationDate deve ser uma data futura.", nameof(dto.ExpirationDate));

    var product = await _productRepository.GetByIdAsync(dto.ProductId);
    if (product == null)
      return null;

    var item = new Item
    {
      ProductId = dto.ProductId,
      Batch = dto.Batch.Trim(),
      Quantity = dto.Quantity,
      ExpirationDate = dto.ExpirationDate,
      Location = dto.Location.Trim(),
      Status = CalculateStatus(dto.Quantity, product.MinimumQuantity)
    };

    await _itemRepository.CreateAsync(item);

    var created = await _itemRepository.GetByIdAsync(item.ItemId);
    return created == null ? null : ItemDto.FromModel(created);
  }

  public async Task<ItemDto?> UpdateItemAsync(int id, ItemUpdateDto dto)
  {
    var item = await _itemRepository.GetByIdForUpdateAsync(id);
    if (item == null)
      return null;

    if (item.Status == 0)
      throw new System.InvalidOperationException("Item inativo nao pode ser atualizado.");

    if (dto.Batch != null)
    {
      if (string.IsNullOrWhiteSpace(dto.Batch))
        throw new System.ArgumentException("Batch nao pode ser vazio.", nameof(dto.Batch));

      item.Batch = dto.Batch.Trim();
    }

    if (dto.ExpirationDate.HasValue)
    {
      if (dto.ExpirationDate.Value <= System.DateTime.Now)
        throw new System.ArgumentException("ExpirationDate deve ser uma data futura.", nameof(dto.ExpirationDate));

      item.ExpirationDate = dto.ExpirationDate.Value;
    }

    if (dto.Location != null)
    {
      if (string.IsNullOrWhiteSpace(dto.Location))
        throw new System.ArgumentException("Location nao pode ser vazia.", nameof(dto.Location));

      item.Location = dto.Location.Trim();
    }

    await _itemRepository.UpdateAsync();

    var updated = await _itemRepository.GetByIdAsync(item.ItemId);
    return updated == null ? null : ItemDto.FromModel(updated);
  }

  public async Task<ItemDeleteResult> DeleteItemAsync(int id)
  {
    var item = await _itemRepository.GetByIdForUpdateAsync(id);
    if (item == null)
      return ItemDeleteResult.NotFound;

    if (item.Status == 0)
      return ItemDeleteResult.AlreadyInactive;

    item.Status = 0;
    await _itemRepository.UpdateAsync();
    return ItemDeleteResult.Deactivated;
  }

  private static sbyte CalculateStatus(int quantity, int? minimumQuantity)
  {
    if (quantity == 0)
      return 3;

    if (minimumQuantity.HasValue && quantity <= minimumQuantity.Value)
      return 2;

    return 1;
  }
}
