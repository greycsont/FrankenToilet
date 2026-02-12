using System.Linq;

namespace FrankenToilet.dolfelive;

public static class ArrayExtensions
{
    public static string ToJoinedString<T>(this T[] array)
    {
        return string.Join(", ", array.Select(i => i?.ToString()));
    }
}