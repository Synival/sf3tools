using System.Collections;
using System.Collections.Generic;

namespace CommonLib {
    public interface IEnumerableLength {
        int Count { get; }
    }

    public interface IEnumerableWithLength : IEnumerable, IEnumerableLength {}
    public interface IEnumerableWithLength<T> : IEnumerable<T>, IEnumerableLength {}
}
