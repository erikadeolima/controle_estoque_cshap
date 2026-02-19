using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace controle_estoque_cshap.Models;

[ExcludeFromCodeCoverage]
public partial class User
{
    public int IdUser { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Profile { get; set; } = null!;

    public virtual ICollection<Movement> Movements { get; set; } = new List<Movement>();
}
