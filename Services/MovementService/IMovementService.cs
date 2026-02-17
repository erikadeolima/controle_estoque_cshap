using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using controle_estoque_cshap.DTOs.MovementDto;

namespace controle_estoque_cshap.Services.MovementService;

public interface IMovementService
{
    Task<List<MovementDto>> GetByItemAsync(int itemId);

    Task<List<MovementDto>> GetByPeriodAsync(DateTime startDate, DateTime endDate);
}
