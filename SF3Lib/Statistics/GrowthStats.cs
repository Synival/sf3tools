using System;
using System.Collections.Generic;
using CommonLib.Statistics;

namespace SF3.Statistics {
    /// <summary>
    /// Utility functions for stats.
    /// </summary>
    public static class GrowthStats {
        /// <summary>
        /// An array for all possible outcomes of the RNG for adding an additional point when gaining stats.
        /// Index: The number below 0x100 used when determining a stat 'growthValue'.
        /// Value: The number of possibilities of the calculation Random(0, 0x7F) + Random(0, 0x7F) out of
        ///        'TotalRngOutcomes' that result in +1 stat.
        /// </summary>
        public static double[] NumRngOutcomesToReachPlusOne = {
                0,     0,     1,     3,     6,    10,    15,    21, // 0x00 - 0x07
               28,    36,    45,    55,    66,    78,    91,   105, // 0x08 - 0x0F
              120,   136,   153,   171,   190,   210,   231,   253, // 0x10 - 0x17
              276,   300,   325,   351,   378,   406,   435,   465, // 0x18 - 0x1F
              496,   528,   561,   595,   630,   666,   703,   741, // 0x20 - 0x27
              780,   820,   861,   903,   946,   990,  1035,  1081, // 0x28 - 0x2F
             1128,  1176,  1225,  1275,  1326,  1378,  1431,  1485, // 0x30 - 0x37
             1540,  1596,  1653,  1711,  1770,  1830,  1891,  1953, // 0x38 - 0x3F
             2016,  2080,  2145,  2211,  2278,  2346,  2415,  2485, // 0x40 - 0x47
             2556,  2628,  2701,  2775,  2850,  2926,  3003,  3081, // 0x48 - 0x4F
             3160,  3240,  3321,  3403,  3486,  3570,  3655,  3741, // 0x50 - 0x57
             3828,  3916,  4005,  4095,  4186,  4278,  4371,  4465, // 0x58 - 0x5F
             4560,  4656,  4753,  4851,  4950,  5050,  5151,  5253, // 0x60 - 0x67
             5356,  5460,  5565,  5671,  5778,  5886,  5995,  6105, // 0x68 - 0x6F
             6216,  6328,  6441,  6555,  6670,  6786,  6903,  7021, // 0x70 - 0x77
             7140,  7260,  7381,  7503,  7626,  7750,  7875,  8001, // 0x78 - 0x7F
             8128,  8256,  8383,  8509,  8634,  8758,  8881,  9003, // 0x80 - 0x87
             9124,  9244,  9363,  9481,  9598,  9714,  9829,  9943, // 0x88 - 0x8F
            10056, 10168, 10279, 10389, 10498, 10606, 10713, 10819, // 0x90 - 0x97
            10924, 11028, 11131, 11233, 11334, 11434, 11533, 11631, // 0x98 - 0x9F
            11728, 11824, 11919, 12013, 12106, 12198, 12289, 12379, // 0xA0 - 0xA7
            12468, 12556, 12643, 12729, 12814, 12898, 12981, 13063, // 0xA8 - 0xAF
            13144, 13224, 13303, 13381, 13458, 13534, 13609, 13683, // 0xB0 - 0xB7
            13756, 13828, 13899, 13969, 14038, 14106, 14173, 14239, // 0xB8 - 0xBF
            14304, 14368, 14431, 14493, 14554, 14614, 14673, 14731, // 0xC0 - 0xC7
            14788, 14844, 14899, 14953, 15006, 15058, 15109, 15159, // 0xC8 - 0xCF
            15208, 15256, 15303, 15349, 15394, 15438, 15481, 15523, // 0xD0 - 0xD7
            15564, 15604, 15643, 15681, 15718, 15754, 15789, 15823, // 0xD8 - 0xDF
            15856, 15888, 15919, 15949, 15978, 16006, 16033, 16059, // 0xE0 - 0xE7
            16084, 16108, 16131, 16153, 16174, 16194, 16213, 16231, // 0xE8 - 0xEF
            16248, 16264, 16279, 16293, 16306, 16318, 16329, 16339, // 0xF0 - 0xF7
            16348, 16356, 16363, 16369, 16374, 16378, 16381, 16383, // 0xF8 - 0xFF
        };

        /// <summary>
        /// The number of possible outcomes for the calculation Random(0, 0x7F) + Random(0, 0x7F).
        /// </summary>
        public const int TotalRngOutcomes = 16384;

        /// <summary>
        /// The level range for a stat gains in a particular group (e.g, HPCurve1 to HPCurve2).
        /// </summary>
        public class StatGrowthGroup {
            public StatGrowthGroup(int groupIndex, bool isPromoted, ValueRange<int> range) {
                GroupIndex = groupIndex;
                IsPromoted = isPromoted;
                Range = range;
            }

            public int GroupIndex { get; }
            public bool IsPromoted { get; }
            public ValueRange<int> Range { get; }
        }

        /// <summary>
        /// Hard-coded list of stat curve groups, indexed by [IsPromoted][GrowthGroup].
        /// </summary>
        public static readonly Dictionary<bool, StatGrowthGroup[]> StatGrowthGroups = new Dictionary<bool, StatGrowthGroup[]>()
        {
            {
                false,
                new StatGrowthGroup[] {
                    new StatGrowthGroup(0, false, new ValueRange<int>(1, 5)),
                    new StatGrowthGroup(1, false, new ValueRange<int>(5, 10)),
                    new StatGrowthGroup(2, false, new ValueRange<int>(10, 12)),
                    new StatGrowthGroup(3, false, new ValueRange<int>(12, 14)),
                    new StatGrowthGroup(4, false, new ValueRange<int>(14, 17)),
                    new StatGrowthGroup(5, false, new ValueRange<int>(17, 30))
                }
            },
            {
                true,
                new StatGrowthGroup[] {
                    new StatGrowthGroup(0, true, new ValueRange<int>(1, 5)),
                    new StatGrowthGroup(1, true, new ValueRange<int>(5, 10)),
                    new StatGrowthGroup(2, true, new ValueRange<int>(10, 15)),
                    new StatGrowthGroup(3, true, new ValueRange<int>(15, 20)),
                    new StatGrowthGroup(4, true, new ValueRange<int>(20, 30)),
                    new StatGrowthGroup(5, true, new ValueRange<int>(30, 99))
                }
            }
        };

        /// <summary>
        /// Returns the average stat gain per level based on an internal growth value provided by
        /// GetStatGrowthValue*() functions.
        /// </summary>
        /// <param name="growthValue">Internal value used for stat growth provided by GetStatGrowthValue*()
        /// functions.</param>
        /// <returns>The average number of stat gains per level.</returns>
        public static double GetAverageStatGrowthPerLevel(int growthValue) {
            var guaranteedStatBonus = (growthValue & 0xf00) % 15;

            // The portion of growthValue % 0x100 is the starting point for the formula to determine whether
            // we should add an additional stat point.
            var growthValuePlusOneCalcStart = Math.Max(growthValue % 0x100, 0);

            // Determine the odds that adding to random numbers range (0x00, 0x7F) will yield a result >= 100,
            // which provides a bonus +1 stat boost.
            var percentToReachPlusOne = NumRngOutcomesToReachPlusOne[growthValuePlusOneCalcStart] / TotalRngOutcomes;

            return percentToReachPlusOne + guaranteedStatBonus;
        }

        /// <summary>
        /// Converts an internal growth value provided by GetStatGrowthValue*() functions into stat gain as a
        /// percentage.
        /// </summary>
        /// <param name="growthValue">Internal value used for stat growth provided by GetStatGrowthValue*()
        /// functions.</param>
        /// <returns>The average number of stat gains per level as a percentage string.</returns>
        public static string GetAverageStatGrowthPerLevelAsPercent(int growthValue)
            => string.Format("{0:0.##}", GetAverageStatGrowthPerLevel(growthValue) * 100) + "%";

        /// <summary>
        /// Returns an internally-used growth value based on a range of stat to increase by over the course of a range
        /// of levels (e.g, +15 HP over 5 levels).
        /// </summary>
        /// <param name="statRange">The total range of stats to increase by over the course of 'levelRange'.</param>
        /// <param name="levelRange">The number of levels over which the stat range is reached.</param>
        /// <returns>An internal value used for calculating stat gains.</returns>
        public static int GetStatGrowthValuePerLevel(int statRange, int levelRange) {
            var statRangeTimes0x100 = statRange << 8;
            switch (levelRange) {
                case 2:
                    return statRangeTimes0x100 >> 1;
                case 3:
                    return statRangeTimes0x100 * 0x100 / 0x300;
                case 4:
                    return statRangeTimes0x100 >> 2;
                case 5:
                    return (statRangeTimes0x100 * 0x100 / 0x280) >> 1;
                case 10:
                    return (statRangeTimes0x100 * 0x100 / 0x280) >> 2;
                case 13:
                    return (statRangeTimes0x100 * 0x100 / 0x340) >> 2;
                case 69:
                    return (statRangeTimes0x100 * 0x100 / 0x228) >> 5;
                default:
                    return statRangeTimes0x100 / levelRange;
            }
        }
    }
}
