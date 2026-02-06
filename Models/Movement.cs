using System;
using System.Collections.Generic;

namespace controle_estoque_cshap.Models;

public partial class Movement
{
    public int MovementId { get; set; }

    public DateTime? Date { get; set; }

    public string Type { get; set; } = null!;

    public int QuantityMoved { get; set; }

    public int PreviousQuantity { get; set; }

    public int NewQuantity { get; set; }

    public int ItemId { get; set; }

    public int UserId { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
