using System;
using System.Linq;
using System.Text;

namespace Common.String
{
    public static class StringExtensions
    {
        public static string RemovePolishDiacritics(this string input)
        {
            var chars = input.Normalize(NormalizationForm.FormD);

            var filteredString = chars.Where(x => char.GetUnicodeCategory(x) != System.Globalization.UnicodeCategory.NonSpacingMark);
            string newString = new string(filteredString.ToArray());
            newString = newString.Replace("ł", "l");

            return newString;
        }

        public static string FirstCharToUpper(this string input)
        {
            return input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
            };
        }

        public static string FirstCharToLower(this string input)
        {
            return input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToLower(), input.AsSpan(1))
            };
        }

        public static string RemoveCharsFromEndOfTheString(this string input, int numberOfStringsToRemove)
            => input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.Remove(input.Length - numberOfStringsToRemove)
            };
    }
}
