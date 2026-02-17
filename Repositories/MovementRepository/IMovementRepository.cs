using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Repositories.MovementRepository;

public interface IMovementRepository
{
    Task<List<Movement>> GetByItemAsync(int itemId);
    Task<List<Movement>> GetByPeriodAsync(DateTime startDate, DateTime endDate);
}
