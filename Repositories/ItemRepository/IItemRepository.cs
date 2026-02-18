using System.Collections.Generic;
using System.Threading.Tasks;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Repositories.ItemRepository;

public interface IItemRepository
{
  Task<IEnumerable<Item>> GetAllAsync();
  Task<Item?> GetByIdAsync(int id);
  Task<Item?> GetByIdForUpdateAsync(int id);
  Task<IEnumerable<Item>> GetByProductIdAsync(int productId);
  Task<IEnumerable<Item>> GetExpiringItemsAsync(int days);
  Task<Item> CreateAsync(Item item);
  Task UpdateAsync(Item item);
  Task DeleteAsync(Item item);
}