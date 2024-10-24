namespace SF3 {
    /// <summary>
    /// Two values - a beginning and an end - with an auto-calculated range.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ValueRange<T> where T : struct {
        public ValueRange(int begin, int end) {
            Begin = begin;
            End = end;
            Range = end - begin;
        }

        public int Begin { get; }
        public int End { get; }
        public int Range { get; }
    }
}
