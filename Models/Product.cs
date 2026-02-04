namespace controle_estoque_cshap.Models;

public enum ProductStatus
{
  Inativo = 0,
  Ativo = 1
}

public class Product
{
  public Guid Id { get; private set; }
  public string SKU { get; private set; } = string.Empty;
  public string Nome { get; set; } = string.Empty;
  public ProductStatus Status { get; private set; }
  public int QuantidadeMinima { get; set; }
  public DateTime DataCriacao { get; private set; }

  public Guid CategoryId { get; set; }

  public Category Category { get; set; } = null!;
  public ICollection<Item> Items { get; set; } = new List<Item>();

  public Product(string sku)
  {
    Id = Guid.NewGuid();
    SKU = sku;
    Status = ProductStatus.Ativo;
    DataCriacao = DateTime.UtcNow;
    QuantidadeMinima = 0;
  }

  private Product() { SKU = string.Empty; }

  public void Ativar()
  {
    Status = ProductStatus.Ativo;
  }

  public void Desativar()
  {
    Status = ProductStatus.Inativo;
  }

  public void ValidarQuantidadeMinima()
  {
    if (QuantidadeMinima < 0)
      throw new ArgumentException("Quantidade mínima não pode ser negativa.");
  }
}
