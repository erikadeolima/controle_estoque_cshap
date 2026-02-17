using System;

namespace controle_estoque_cshap.DTOs.ItemDto;

public class ExpirationReportDto
{
  public int ItemId { get; set; }
  public string Batch { get; set; } = null!;
  public int Quantity { get; set; }
  public string Location { get; set; } = null!;
  public DateTime? ExpirationDate { get; set; }
  public string ProductName { get; set; } = null!;
  public string ProductSku { get; set; } = null!;
  public string CategoryName { get; set; } = null!;
  public string Status { get; set; } = null!; // "VENCIDO" ou "PRÓXIMO A VENCER"
  public int DaysUntilExpiration { get; set; }
}
