using System.Globalization;

namespace controle_estoque_cshap.Utils;

public static class NameValidation
{
  private static readonly CultureInfo PtBrCulture = new("pt-BR");

  public static bool HasLeadingOrTrailingSpaces(string name)
  {
    var trimmedName = name.Trim();
    return name != trimmedName;
  }

  public static bool ContainsOnlyLettersAndSpaces(string name)
  {
    foreach (var ch in name)
    {
      if (!char.IsLetter(ch) && ch != ' ')
        return false;
    }

    return true;
  }

  public static string NormalizeTitleCase(string name)
  {
    var trimmedName = name.Trim();
    var lower = trimmedName.ToLower(PtBrCulture);
    // Se o sistema for multinacional, considere receber locale/pais para normalizar com a cultura correta.
    return PtBrCulture.TextInfo.ToTitleCase(lower);
  }
}
