using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using controle_estoque_cshap.DTOs.ItemDto;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories;
using controle_estoque_cshap.Repositories.ItemRepository;

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
    var product = await _productRepository.GetByIdAsync(dto.ProductId);
    if (product == null)
      return null;

    var item = new Item
    {
      ProductId = dto.ProductId,
      Batch = dto.Batch.Trim(),
      Quantity = dto.Quantity,
      ExpirationDate = dto.ExpirationDate,
      Location = dto.Location,
      Status = 1
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

    if (dto.Batch != null)
      item.Batch = dto.Batch.Trim();

    if (dto.Quantity.HasValue)
      item.Quantity = dto.Quantity.Value;

    if (dto.ExpirationDate.HasValue)
      item.ExpirationDate = dto.ExpirationDate.Value;

    if (dto.Location != null)
      item.Location = dto.Location;

    if (dto.Status.HasValue)
      item.Status = dto.Status.Value;

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
}
