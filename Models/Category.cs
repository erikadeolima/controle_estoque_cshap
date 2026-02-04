namespace controle_estoque_cshap.Models;

public class Category
{
  public Guid Id { get; private set; }
  public string Nome { get; set; } = string.Empty;
  public string? Descricao { get; set; }
  public DateTime DataCriacao { get; private set; }

  // Navigation property
  public ICollection<Product> Products { get; set; } = new List<Product>();

  public Category()
  {
    Id = Guid.NewGuid();
    DataCriacao = DateTime.UtcNow;
  }
}
