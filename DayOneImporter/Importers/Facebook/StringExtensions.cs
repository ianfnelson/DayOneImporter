using System.Text;

namespace DayOneImporter.Importers.Facebook;

public static class StringExtensions
{
    public static string FixFacebookEncoding(this string input)
    {
        var noHandsInAir = input.Replace("\\o/", "[hands_in_air]");

        var targetEncoding = Encoding.GetEncoding("ISO-8859-1");
        var unescapeText = System.Text.RegularExpressions.Regex.Unescape(noHandsInAir);
        return Encoding.UTF8.GetString(targetEncoding.GetBytes(unescapeText));
    }
}