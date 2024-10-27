using SF3.Types;
using static SF3.Tests.TestDataPaths;

namespace SF3.Tests {
    internal class TestCase {
        public TestCase(ScenarioType scenario, string filename) {
            Scenario = scenario;
            Filename = ResourcePath(scenario, filename);
        }

        public ScenarioType Scenario { get; }
        public string Filename { get; }
        public virtual string Name => Filename + " (" + Scenario.ToString() + ")";

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
