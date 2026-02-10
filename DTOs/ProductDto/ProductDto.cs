using System;

namespace controle_estoque_cshap.DTOs.ProductDto;
public class ProductDto
{
    public int ProductId { get; set; }

    public string Sku { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public sbyte? Status { get; set; }

    public int? MinimumQuantity { get; set; }

    public DateTime? CreationDate { get; set; }

    public int CategoryId { get; set; }

    public int QuantityTotal { get; set; }
}
