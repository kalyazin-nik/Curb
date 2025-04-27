using System.Text.RegularExpressions;

namespace Curb.API.Extensions;

internal static class StringExtensions
{
    public static string ToStringSnakeCase(this string name)
    {
        return Regex.Replace(name.ToString(), "(?<!^)([A-Z])", "_$1").ToLower();
    }
}
