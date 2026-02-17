using NUnit.Framework;
using controle_estoque_cshap.Utils;

namespace controle_estoque_cshap.Tests;

public class NameValidationTests
{
  [TestCase(" Nome ", true)]
  [TestCase("Nome", false)]
  public void HasLeadingOrTrailingSpaces_DetectsSpaces(string input, bool expected)
  {
    var result = NameValidation.HasLeadingOrTrailingSpaces(input);
    Assert.That(result, Is.EqualTo(expected));
  }

  [TestCase("Nome", true)]
  [TestCase("Nome 123", false)]
  [TestCase("Nome-Produto", false)]
  public void ContainsOnlyLettersAndSpaces_ValidatesInput(string input, bool expected)
  {
    var result = NameValidation.ContainsOnlyLettersAndSpaces(input);
    Assert.That(result, Is.EqualTo(expected));
  }

  [Test]
  public void NormalizeTitleCase_TrimsAndNormalizes()
  {
    var result = NameValidation.NormalizeTitleCase("  pao de queijo  ");
    Assert.That(result, Is.EqualTo("Pao De Queijo"));
  }
}
