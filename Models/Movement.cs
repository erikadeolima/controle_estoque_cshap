namespace controle_estoque_cshap.Models;

public enum MovementType
{
  Entrada,
  Saida,
  Ajuste
}

public class Movement
{
  public Guid Id { get; private set; }
  public DateTime Data { get; private set; }
  public MovementType Tipo { get; private set; }
  public int QuantidadeMovimentada { get; private set; }
  public int QuantidadeAnterior { get; private set; }
  public int QuantidadeNova { get; private set; }

  // Foreign Keys
  public Guid ItemId { get; private set; }
  public Guid UserId { get; private set; }

  // Navigation properties
  public Item Item { get; set; } = null!;
  public User User { get; set; } = null!;

  public Movement(
      MovementType tipo,
      int quantidadeMovimentada,
      int quantidadeAnterior,
      int quantidadeNova,
      Guid itemId,
      Guid userId)
  {
    Id = Guid.NewGuid();
    Data = DateTime.UtcNow;
    Tipo = tipo;
    QuantidadeMovimentada = quantidadeMovimentada;
    QuantidadeAnterior = quantidadeAnterior;
    QuantidadeNova = quantidadeNova;
    ItemId = itemId;
    UserId = userId;
  }

  // Para EF Core
  private Movement() { }
}
