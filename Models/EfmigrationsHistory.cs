using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace controle_estoque_cshap.Models;

[ExcludeFromCodeCoverage]
public partial class EfmigrationsHistory
{
    public string MigrationId { get; set; } = null!;

    public string ProductVersion { get; set; } = null!;
}
