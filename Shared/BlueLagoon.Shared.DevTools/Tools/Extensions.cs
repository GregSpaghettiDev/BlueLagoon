using System.Text;

namespace BlueLagoon.Shared.DevTools.Tools;

public static class Extensions
{
    public static string RemovePolishDiacritics(this string input)
    {
        var chars = input.Normalize(NormalizationForm.FormD);

        var filteredString = chars.Where(x => char.GetUnicodeCategory(x) != System.Globalization.UnicodeCategory.NonSpacingMark);
        string newString = new(filteredString.ToArray());
        newString = newString.Replace("ł", "l");

        return newString;
    }
}
