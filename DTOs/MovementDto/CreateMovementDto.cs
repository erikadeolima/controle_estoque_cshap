namespace controle_estoque_cshap.DTOs.MovementDto;

public class CreateMovementDto
{
    public int ItemId { get; set; }
    public int UserId { get; set; }
    public int Quantity { get; set; }
    public string Type { get; set; } = null!;
}
