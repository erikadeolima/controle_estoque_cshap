using System;

namespace controle_estoque_cshap.DTOs.CategoryDto;

public class CategoryDto
{
  public int CategoryId { get; set; }

  public string Name { get; set; } = string.Empty;

  public string? Description { get; set; }

  public DateTime? CreationDate { get; set; }

  public int TotalProducts { get; set; }
}
