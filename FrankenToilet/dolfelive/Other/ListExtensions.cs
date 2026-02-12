using System.Collections.Generic;
using System.Linq;

namespace FrankenToilet.dolfelive;

public static class ListExtensions
{
    public static string ToJoinedString<T>(this List<T> list)
    {
        return string.Join(", ", list.Select(_ => _?.ToString()));
    }
}
