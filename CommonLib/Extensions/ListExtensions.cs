using System.Collections;
using System.Collections.Generic;

namespace CommonLib.Extensions {
    public static class ListExtensions {
        private class ListWithLength<T> : IEnumerableWithLength<T> {
            public ListWithLength(List<T> list) {
                List = list;
            }

            public List<T> List { get; }

            public int Count => List.Count;
            public IEnumerator<T> GetEnumerator() => List.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public static IEnumerableWithLength<T> ToEnumerableWithLength<T>(this List<T> list)
            => new ListWithLength<T>(list);
    }
}
