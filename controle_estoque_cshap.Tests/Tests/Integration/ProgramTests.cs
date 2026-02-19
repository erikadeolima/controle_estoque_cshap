using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace controle_estoque_cshap.Tests;

public class ProgramTests
{
  [Test]
  public async Task SwaggerEndpoint_Responds_WhenDevelopment()
  {
    await using var factory = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(builder => builder.UseEnvironment("Development"));

    var client = factory.CreateClient(new WebApplicationFactoryClientOptions
    {
      AllowAutoRedirect = false
    });

    var response = await client.GetAsync("/swagger/index.html");

    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK).Or.EqualTo(HttpStatusCode.TemporaryRedirect));
  }
}
