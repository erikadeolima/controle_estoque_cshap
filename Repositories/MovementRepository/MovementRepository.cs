using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using controle_estoque_cshap.Data;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Repositories.MovementRepository;

public class MovementRepository : IMovementRepository
{
    private readonly AppDbContext _context;

    public MovementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Movement>> GetByItemAsync(int itemId)
    {
        return await _context.Movements
            .AsNoTracking()
            .Include(m => m.Item)
                .ThenInclude(i => i.Product)
            .Include(m => m.User)
            .Where(m => m.ItemId == itemId)
            .OrderByDescending(m => m.Date)
            .ToListAsync();
    }

    public async Task<List<Movement>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Movements
            .AsNoTracking()
            .Include(m => m.Item)
                .ThenInclude(i => i.Product)
            .Include(m => m.User)
            .Where(m =>
                m.Date >= startDate &&
                m.Date <= endDate)
            .OrderByDescending(m => m.Date)
            .ToListAsync();
    }
}
