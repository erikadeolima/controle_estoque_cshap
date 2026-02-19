using System.Collections.Generic;
using System.Threading.Tasks;
using controle_estoque_cshap.DTOs.ItemDto;

namespace controle_estoque_cshap.Services.ItemService;

public interface IItemService
{
  Task<IEnumerable<ItemDto>> GetAllAsync();
  Task<ItemDto?> GetItemByIdAsync(int id);
  Task<IEnumerable<ItemDto>> GetByProductIdAsync(int productId);
  Task<IEnumerable<ItemDto>> GetExpiringItemsAsync(int days);
  Task<ItemDto?> CreateItemAsync(ItemCreateDto dto);
  Task<ItemDto?> UpdateItemAsync(int id, ItemUpdateDto dto);
  Task<ItemDeleteResult> DeleteItemAsync(int id);
  Task<string> GenerateExpirationReportCsvAsync(int daysToWarning);
  Task<string> GenerateExpiredItemsReportCsvAsync();
}