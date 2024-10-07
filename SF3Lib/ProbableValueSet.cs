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

        /// <summary>
        /// Calculates the weighted median of the probability set, but at a certain position where the distribution
        /// isn't 50% but any other percentage.
        /// </summary>
        /// <param name="targetProbability">The target percentage of the distribution area for the value we're looking for.</param>
        /// <returns>A linearly-interpolated weighted median at a specific target distribution percent.</returns>
        public double GetWeightedMedianAt(double targetProbability)
        {
            if (this.Count == 0)
            {
                throw new IndexOutOfRangeException();
            }
            if (this.Count == 1)
            {
                return this.ToArray()[0].Key;
            }

            var sortedValues = this.OrderBy(x => x.Key).ToArray();
            if (targetProbability <= 0.00)
            {
                return sortedValues[0].Key;
            }
            if (targetProbability >= 1.00)
            {
                return sortedValues[sortedValues.Length - 1].Key;
            }

            // Make sure the target reflects a percentage of all total probabilities.
            double totalProbability = sortedValues.Sum(kv => kv.Value);
            targetProbability *= totalProbability;

            // For a funky linearly-interpolated median, we'll iterate over the value by (n-1)/n to transform ranges like this:
            //     [0.00, 1.00, 2.00]
            // To ranges like this:
            //     [0.00, 0.66, 1.33, 2.00]

            double n = 0.0;
            double nInterval = (double)(sortedValues.Length - 1) / (double)sortedValues.Length;

            // Track the total probability we've passed.
            double currentProbability = 0.00;

            for (int i = 0; i <= sortedValues.Length; i++, n += nInterval)
            {
                // If we haven't reached the target probability yet, keep going.
                double segmentProbability = sortedValues[i].Value;
                if (currentProbability + segmentProbability < targetProbability)
                {
                    currentProbability += segmentProbability;
                    continue;
                }

                // Determine the interpolation position of 'targetProbability' between 'currentProbability' and ('currentProbability' + 'segmentProbability').
                double probabilityInterp = (targetProbability - currentProbability) / segmentProbability;

                // Increase the index we're using by an interval proportional to the interpolated position in the probability segment.
                n += probabilityInterp * nInterval;

                // Check for some simple cases.
                int intN = (int)n;
                if (intN >= sortedValues.Length)
                {
                    return sortedValues[sortedValues.Length - 1].Key;
                }
                if (n == (double)intN)
                {
                    return sortedValues[(int)n].Key;
                }

                // Return a value[n], interpolating between 'n' and 'n + 1' for the fractional portion of 'n'
                double valueInterp = n - (double)intN;
                double value1 = sortedValues[intN].Key;
                double value2 = sortedValues[intN + 1].Key;
                double valueRange = value2 - value1;
                return value1 + valueRange * valueInterp;
            }

            // Looks like we exceed the set - use the last value.
            return sortedValues[sortedValues.Length - 1].Key;
        }

        /// <summary>
        /// Calculates the weighted median of the probability set.
        /// </summary>
        /// <returns>A linearly-interpolated weighted median of all values in the probability set.</returns>
        public double GetWeightedMedian()
        {
            return GetWeightedMedianAt(0.50);
        }
    }
}
