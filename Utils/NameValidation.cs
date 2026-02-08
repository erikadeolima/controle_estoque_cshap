using System.Globalization;

namespace controle_estoque_cshap.Utils;

public static class NameValidation
{
  private static readonly CultureInfo PtBrCulture = new("pt-BR");

  public static bool HasLeadingOrTrailingSpaces(string name)
  {
    var nameWithoutWithSpaces = name.Trim();
    return name != nameWithoutWithSpaces;
  }

  public static bool ContainsOnlyLettersAndSpaces(string name)
  {
    foreach (var ch in name)
    {
      if (!char.IsLetter(ch) && !char.IsWhiteSpace(ch))
        return false;
    }

    return true;
  }

  public static string NormalizeTitleCase(string name)
  {
    var nameWithoutWithSpace = name.Trim();
    var lower = nameWithoutWithSpace.ToLower(PtBrCulture);
    // Se o sistema for multinacional, considere receber locale/pais para normalizar com a cultura correta.
    return PtBrCulture.TextInfo.ToTitleCase(lower);
  }
}
