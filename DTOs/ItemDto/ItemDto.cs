namespace controle_estoque_cshap.DTOs.ItemDto;

public class ItemDto
{
  public int ItemId { get; set; }
  public int ProductId { get; set; }
  public string ProductName { get; set; } = string.Empty;
  public string Batch { get; set; } = string.Empty;
  public int Quantity { get; set; }
  public DateTime? ExpirationDate { get; set; }
  public string? Location { get; set; }
  public sbyte? Status { get; set; }

  public static ItemDto FromModel(Models.Item item)
  {
    return new ItemDto
    {
      ItemId = item.ItemId,
      ProductId = item.ProductId,
      ProductName = item.Product?.Name ?? string.Empty,
      Batch = item.Batch,
      Quantity = item.Quantity,
      ExpirationDate = item.ExpirationDate,
      Location = item.Location,
      Status = item.Status
    };
  }
}