using System.Collections.Generic;

namespace HassIconGenerator
{
    public static class ExtensionMethods
    {
        public static IEnumerable<T> FilterNulls<T>(this IEnumerable<T?> source) where T : class
        {
            foreach (var item in source)
            {
                if (item != null)
                {
                    yield return item;
                }
                continue;
            }
        }
    }
}
