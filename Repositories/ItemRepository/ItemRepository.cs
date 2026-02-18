using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using controle_estoque_cshap.Data;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Repositories.ItemRepository;

public class ItemRepository : IItemRepository
{
  private readonly AppDbContext _context;

  public ItemRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Item>> GetAllAsync()
  {
    return await _context.Items
        .AsNoTracking()
        .Include(i => i.Product)
        .ToListAsync();
  }

  public async Task<Item?> GetByIdAsync(int id)
  {
    return await _context.Items
        .AsNoTracking()
        .Include(i => i.Product)
        .FirstOrDefaultAsync(i => i.ItemId == id);
  }

  public async Task<Item?> GetByIdForUpdateAsync(int id)
  {
    return await _context.Items
        .FirstOrDefaultAsync(i => i.ItemId == id);
  }

  public async Task<IEnumerable<Item>> GetByProductIdAsync(int productId)
  {
    return await _context.Items
        .AsNoTracking()
        .Include(i => i.Product)
        .Where(i => i.ProductId == productId)
        .ToListAsync();
  }

  public async Task<IEnumerable<Item>> GetExpiringItemsAsync(int days)
  {
    var today = DateTime.UtcNow.Date;
    var targetDate = today.AddDays(days);

    return await _context.Items
        .AsNoTracking()
        .Include(i => i.Product)
        .Where(i => i.ExpirationDate.HasValue && i.ExpirationDate.Value.Date <= targetDate)
        .ToListAsync();
  }

  public async Task<Item> CreateAsync(Item item)
  {
    _context.Items.Add(item);
    await _context.SaveChangesAsync();
    return item;
  }

  public async Task UpdateAsync(Item item)
{
    _context.Items.Update(item);
    await _context.SaveChangesAsync();
}
  public async Task DeleteAsync(Item item)
  {
    _context.Items.Remove(item);
    await _context.SaveChangesAsync();
  }
}