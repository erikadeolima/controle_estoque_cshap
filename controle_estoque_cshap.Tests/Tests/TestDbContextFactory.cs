using Microsoft.EntityFrameworkCore;
using controle_estoque_cshap.Data;

namespace controle_estoque_cshap.Tests;

public static class TestDbContextFactory
{
  public static AppDbContext Create(string databaseName)
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName)
      .Options;

    var context = new AppDbContext(options);
    context.Database.EnsureCreated();
    return context;
  }
}
