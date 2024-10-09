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
        /// Calculates a linearly-interpreted weighted median-like value for probability set at an arbitrary position
        /// (not fixed at the middle point like the median).
        /// </summary>
        /// <param name="targetArea">The total area left of the median-like value we're looking for.
        /// In range (0.00, 1.00).</param>
        /// <returns>A linearly-interpolated weighted median where the area left of the result is equal to 'targetArea'.</returns>
        public double GetWeightedMedianAt(double targetArea)
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
            if (targetArea <= 0.00)
            {
                return sortedValues[0].Key;
            }
            if (targetArea >= 1.00)
            {
                return sortedValues[sortedValues.Length - 1].Key;
            }

            // Make sure the target reflects a percentage of all total probabilities.
            double totalArea = sortedValues.Sum(kv => kv.Value);
            targetArea *= totalArea;

            // Track the total area we've passed.
            double currentArea = 0.00;

            for (int i = 0; i < sortedValues.Length; i++)
            {
                // If we haven't reached the target probability yet, keep going.
                double probability = sortedValues[i].Value;
                if (currentArea + probability < targetArea)
                {
                    currentArea += probability;
                    continue;
                }

                // Determine the interpolation percentage of 'targetArea' between (currentArea, currentArea + probability).
                double probabilityInterp = (targetArea - currentArea) / probability;

                // Determine an index of the dataset with the fractional portion. This will be used for linear interpolation.
                double n = (double)i + probabilityInterp;

                // The domain for linear interpolation is larger than the domain of the input dataset, so need to convert from a domain like this:
                // (example for 3 data points):
                //     (0.00, 1.00, 2.00, 3.00)
                // to: (0.00, 0.66, 1.33, 2.00)
                n *= (double)(sortedValues.Length - 1) / (double)sortedValues.Length;

                // Check for some simple cases, and cases potentially caused by floating point imprecision.
                double nFloor = Math.Floor(n);
                int nInt = (int)nFloor;
                if (n == nFloor)
                {
                    return sortedValues[sortedValues.Length - 1].Key;
                }
                else if (nInt >= sortedValues.Length)
                {
                    return sortedValues[(int)nFloor].Key;
                }

                // Return a value[n], interpolating between 'n' and 'n + 1' for the fractional portion of 'n'
                double valueInterp = n - nFloor;
                double value1 = sortedValues[nInt].Key;
                double value2 = sortedValues[nInt + 1].Key;
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
