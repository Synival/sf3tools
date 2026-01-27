namespace CommonLib {
    public interface IIndexedEnumerableWithLength<T> : IEnumerableWithLength<T> {
        T this[int index] { get; }
        T[] AsArray();
    }
}
