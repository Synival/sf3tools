using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3
{
    /// <summary>
    /// A set of values paired with a probability in the form of a key-value dictionary.
    /// Key: the value
    /// Value: the probability of the value
    /// </summary>
    public class ProbableValueSet : Dictionary<int, double>
    {
        /// <summary>
        /// Produces a new ProbableValueSet with the aggregate of all its original values transformed into
        /// individual ProbableValueSet's. This is useful if you want to apply a transformation, like adding 1 for
        /// landing "Heads" on a coin flip, "n" number of times to see the results after "n" number of coin flips.
        /// </summary>
        /// <param name="transformFunc">Takes an input value and returns a ProbableValueSet for all possible results.</param>
        /// <returns>A ProbablyValueSet with an aggregate of all results of "transformFunc" for all original values.</returns>
        public ProbableValueSet RollNext(Func<int, ProbableValueSet> transformFunc)
        {
            var resultSet = new ProbableValueSet();

            foreach (var keyValueIn in this)
            {
                var valueIn = keyValueIn.Key;
                double probabilityIn = keyValueIn.Value;
                if (probabilityIn == 0.00)
                {
                    continue;
                }

                var transformedSet = transformFunc(valueIn);
                foreach (var keyValueOut in transformedSet)
                {
                    var valueOut = keyValueOut.Key;
                    double probabilityOut = keyValueOut.Value;
                    if (probabilityOut == 0.00)
                    {
                        continue;
                    }

                    if (!resultSet.ContainsKey(valueOut))
                    {
                        resultSet.Add(valueOut, probabilityIn * probabilityOut);
                    }
                    else
                    {
                        resultSet[valueOut] += probabilityIn * probabilityOut;
                    }
                }
            }

            return resultSet;
        }

        /// <summary>
        /// Calculates the weighted average of the probability set.
        /// </summary>
        /// <returns>The weighted average of all values in the probability set.</returns>
        public double GetWeightedAverage()
        {
            if (this.Count == 0)
            {
                throw new IndexOutOfRangeException();
            }
            if (this.Count == 1)
            {
                return this.ToArray()[0].Key;
            }

            return this.Sum(kv => kv.Key * kv.Value);
        }
    }
}
