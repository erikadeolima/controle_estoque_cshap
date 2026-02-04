namespace controle_estoque_cshap.Models;

public class User
{
  public Guid Id { get; private set; }
  public string Nome { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Perfil { get; set; } = string.Empty;

  public ICollection<Movement> Movements { get; set; } = new List<Movement>();

  public User()
  {
    Id = Guid.NewGuid();
  }
}
