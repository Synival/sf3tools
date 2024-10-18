using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3LibTests
{
    public static class Utils
    {
        public static void RunTestCases<T>(IEnumerable<T> testCases, Action<T> test)
        {
            var exceptions = new List<Exception>();

            foreach (var testCase in testCases)
            {
                try
                {
                    test(testCase);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}
