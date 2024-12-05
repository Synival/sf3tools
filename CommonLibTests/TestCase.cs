namespace CommonLib.Tests {
    public class TestCase {
        public TestCase(string name) {
            Name = name;
        }

        public string Name { get; }

        public static void Run<T>(IEnumerable<T> testCases, Action<T> test) where T : TestCase {
            var exceptions = new List<(T, Exception)>();

            foreach (var testCase in testCases) {
                try {
                    test(testCase);
                }
                catch (Exception ex) {
                    exceptions.Add((testCase, ex));
                }
            }

            if (exceptions.Any()) {
                throw new AggregateException(
                    "\n" + string.Join('\n', exceptions.Select(x => x.Item1.Name + ": " + x.Item2.Message)) + "\n",
                    exceptions.Select(x => x.Item2).ToArray());
            }
        }
    }
}
