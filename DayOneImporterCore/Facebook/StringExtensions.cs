using System.Text;

namespace DayOneImporterCore.Facebook;

public static class StringExtensions
{
    public static string FixFacebookEncoding(this string input)
    {
        var targetEncoding = Encoding.GetEncoding("ISO-8859-1");
        var unescapeText = System.Text.RegularExpressions.Regex.Unescape(input);
        return Encoding.UTF8.GetString(targetEncoding.GetBytes(unescapeText));
    }
}