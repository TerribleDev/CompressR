using System.Collections.Generic;

namespace CompressR.MVC
{
    public static class Extensions
    {
        public static IEnumerable<string> Trim(this IEnumerable<string> arr)
        {
            foreach(var item in arr)
            {
                yield return item.Trim();
            }
        }
    }
}