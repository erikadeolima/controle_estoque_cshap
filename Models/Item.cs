using System;
using System.Collections.Generic;

namespace controle_estoque_cshap.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string Batch { get; set; } = null!;

    public DateTime? ExpirationDate { get; set; }

    public int Quantity { get; set; }

    public string? Location { get; set; }

    public sbyte? Status { get; set; }

    public int ProductId { get; set; }

    public virtual ICollection<Movement> Movements { get; set; } = new List<Movement>();

    public virtual Product Product { get; set; } = null!;
}
