using System.Collections;
using System.Collections.Generic;

namespace CommonLib {
    public interface IEnumerableLength {
        int Length { get; }
    }

    public interface IEnumerableWithLength : IEnumerable, IEnumerableLength {}
    public interface IEnumerableWithLength<T> : IEnumerable<T>, IEnumerableLength {}
}
