using System;
using System.Collections.Generic;

namespace controle_estoque_cshap.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Sku { get; set; } = null!;

    public string Name { get; set; } = null!;

    public sbyte? Status { get; set; }

    public int? MinimumQuantity { get; set; }

    public DateTime? CreationDate { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
