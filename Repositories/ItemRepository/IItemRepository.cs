using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Repositories.ItemRepository;

public class ExpirationReportData
{
  public int ItemId { get; set; }
  public string Batch { get; set; } = null!;
  public int Quantity { get; set; }
  public string Location { get; set; } = null!;
  public DateTime? ExpirationDate { get; set; }
  public string ProductName { get; set; } = null!;
  public string ProductSku { get; set; } = null!;
  public string CategoryName { get; set; } = null!;
}

public interface IItemRepository
{
  Task<IEnumerable<Item>> GetAllAsync();
  Task<Item?> GetByIdAsync(int id);
  Task<Item?> GetByIdForUpdateAsync(int id);
  Task<IEnumerable<Item>> GetByProductIdAsync(int productId);
  Task<IEnumerable<Item>> GetExpiringItemsAsync(int days);
  Task<IEnumerable<ExpirationReportData>> GetExpirationReportAsync(int daysToWarning);
  Task<IEnumerable<ExpirationReportData>> GetExpiredItemsReportAsync();
  Task<Item> CreateAsync(Item item);
  Task UpdateAsync(Item item);
  Task DeleteAsync(Item item);
}