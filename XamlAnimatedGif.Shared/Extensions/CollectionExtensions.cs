
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XamlAnimatedGif.Extensions
{
    static class CollectionExtensions
    {
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list)
        {
            return new ReadOnlyCollection<T>(list);
        }
    }
}
