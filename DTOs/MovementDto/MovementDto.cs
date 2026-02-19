using System;

namespace controle_estoque_cshap.DTOs.MovementDto;

public class MovementDto
{
    public int MovementId { get; set; }
    public DateTime Date { get; set; }
    public string? Type { get; set; }

    public int QuantityMoved { get; set; }

    public int PreviousQuantity { get; set; }

    public int NewQuantity { get; set; }

    public int ItemId { get; set; }

    public string? ProductName { get; set; }

    public int UserId { get; set; }

    public string? UserName { get; set; }
}
