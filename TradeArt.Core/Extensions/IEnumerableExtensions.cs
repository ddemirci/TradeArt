using System.Collections.Generic;
using System.Linq;

namespace TradeArt.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> fullList, int batchSize)
        {
            int total = 0;
            while (total < fullList.Count())
            {
                yield return fullList.Skip(total).Take(batchSize);
                total += batchSize;
            }
        }
    }
}
