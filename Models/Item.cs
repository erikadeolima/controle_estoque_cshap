namespace controle_estoque_cshap.Models;

public enum ItemStatus
{
  Disponivel,
  Alerta,
  Esgotado
}

public class Item
{
  public Guid Id { get; private set; }
  public string Batch { get; set; } = string.Empty;
  public DateTime? DataValidade { get; set; }
  public int Quantidade { get; private set; }
  public string Localizacao { get; set; } = string.Empty;
  public ItemStatus Status { get; private set; }
  public DateTime DataCriacao { get; private set; }

  // Foreign Key
  public Guid ProductId { get; set; }

  // Navigation properties
  public Product Product { get; set; } = null!;
  public ICollection<Movement> Movements { get; set; } = new List<Movement>();

  public Item()
  {
    Id = Guid.NewGuid();
    DataCriacao = DateTime.UtcNow;
    Quantidade = 0;
    Status = ItemStatus.Esgotado;
  }

  public void AdicionarQuantidade(int qtd)
  {
    if (qtd <= 0)
      throw new ArgumentException("Quantidade deve ser maior que zero.");

    Quantidade += qtd;
    AtualizarStatus();
  }

  public void RemoverQuantidade(int qtd)
  {
    if (qtd <= 0)
      throw new ArgumentException("Quantidade deve ser maior que zero.");

    if (Quantidade < qtd)
      throw new InvalidOperationException("Estoque insuficiente.");

    Quantidade -= qtd;
    AtualizarStatus();
  }

  public void AtualizarStatus()
  {
    if (Quantidade == 0)
    {
      Status = ItemStatus.Esgotado;
      return;
    }

    if (Product != null && Quantidade <= Product.QuantidadeMinima)
    {
      Status = ItemStatus.Alerta;
      return;
    }

    Status = ItemStatus.Disponivel;
  }

  public void ValidarDataValidade()
  {
    if (DataValidade.HasValue && DataValidade.Value <= DateTime.UtcNow)
      throw new ArgumentException("Data de validade deve ser futura.");
  }
}
