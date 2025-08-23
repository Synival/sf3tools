using System;
using CommonLib.Attributes;
using CommonLib.Statistics;
using SF3.Statistics;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
    public class StatStatistics : Struct {
        /// <summary>
        /// When enabled, GetAverageStatGrowthPerLevelAsPercent() will show the "growthValue" in its output
        /// </summary>
        public static bool DebugGrowthValues { get; set; } = false;

        /// <summary>
        /// *Mathematical* statistics for *character* stats for an individual character at a specific promotion.
        /// </summary>
        /// <param name="stats">Character stats to generate mathematical statistics from.</param>
        public StatStatistics(Stats stats)
        : base(stats.Data, stats.ID, stats.Name + " (Stats)", stats.Address, stats.Size) {
            Stats = stats;
        }

        public Stats Stats { get; }

        public ValueRange<int> GetStatGrowthRange(StatType stat, int groupIndex) {
            switch (stat) {
                case StatType.HP:
                    switch (groupIndex) {
                        case 0: return new ValueRange<int>(Stats.HPCurve1,     Stats.HPCurve5);
                        case 1: return new ValueRange<int>(Stats.HPCurve5,     Stats.HPCurve10);
                        case 2: return new ValueRange<int>(Stats.HPCurve10,    Stats.HPCurve12_15);
                        case 3: return new ValueRange<int>(Stats.HPCurve12_15, Stats.HPCurve14_20);
                        case 4: return new ValueRange<int>(Stats.HPCurve14_20, Stats.HPCurve17_30);
                        case 5: return new ValueRange<int>(Stats.HPCurve17_30, Stats.HPCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case StatType.MP:
                    switch (groupIndex) {
                        case 0: return new ValueRange<int>(Stats.MPCurve1,     Stats.MPCurve5);
                        case 1: return new ValueRange<int>(Stats.MPCurve5,     Stats.MPCurve10);
                        case 2: return new ValueRange<int>(Stats.MPCurve10,    Stats.MPCurve12_15);
                        case 3: return new ValueRange<int>(Stats.MPCurve12_15, Stats.MPCurve14_20);
                        case 4: return new ValueRange<int>(Stats.MPCurve14_20, Stats.MPCurve17_30);
                        case 5: return new ValueRange<int>(Stats.MPCurve17_30, Stats.MPCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case StatType.Atk:
                    switch (groupIndex) {
                        case 0: return new ValueRange<int>(Stats.AtkCurve1,     Stats.AtkCurve5);
                        case 1: return new ValueRange<int>(Stats.AtkCurve5,     Stats.AtkCurve10);
                        case 2: return new ValueRange<int>(Stats.AtkCurve10,    Stats.AtkCurve12_15);
                        case 3: return new ValueRange<int>(Stats.AtkCurve12_15, Stats.AtkCurve14_20);
                        case 4: return new ValueRange<int>(Stats.AtkCurve14_20, Stats.AtkCurve17_30);
                        case 5: return new ValueRange<int>(Stats.AtkCurve17_30, Stats.AtkCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case StatType.Def:
                    switch (groupIndex) {
                        case 0: return new ValueRange<int>(Stats.DefCurve1,     Stats.DefCurve5);
                        case 1: return new ValueRange<int>(Stats.DefCurve5,     Stats.DefCurve10);
                        case 2: return new ValueRange<int>(Stats.DefCurve10,    Stats.DefCurve12_15);
                        case 3: return new ValueRange<int>(Stats.DefCurve12_15, Stats.DefCurve14_20);
                        case 4: return new ValueRange<int>(Stats.DefCurve14_20, Stats.DefCurve17_30);
                        case 5: return new ValueRange<int>(Stats.DefCurve17_30, Stats.DefCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case StatType.Agi:
                    switch (groupIndex) {
                        case 0: return new ValueRange<int>(Stats.AgiCurve1,     Stats.AgiCurve5);
                        case 1: return new ValueRange<int>(Stats.AgiCurve5,     Stats.AgiCurve10);
                        case 2: return new ValueRange<int>(Stats.AgiCurve10,    Stats.AgiCurve12_15);
                        case 3: return new ValueRange<int>(Stats.AgiCurve12_15, Stats.AgiCurve14_20);
                        case 4: return new ValueRange<int>(Stats.AgiCurve14_20, Stats.AgiCurve17_30);
                        case 5: return new ValueRange<int>(Stats.AgiCurve17_30, Stats.AgiCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public double GetAverageStatGrowthPerLevel(StatType stat, int groupIndex) {
            var growthValue = GrowthStats.GetStatGrowthValuePerLevel(GetStatGrowthRange(stat, groupIndex).Range, GrowthStats.StatGrowthGroups[Stats.IsPromoted][groupIndex].Range.Range);
            return GrowthStats.GetAverageStatGrowthPerLevel(growthValue);
        }

        public string GetAverageStatGrowthPerLevelAsPercent(StatType stat, int groupIndex) {
            var growthValue = GrowthStats.GetStatGrowthValuePerLevel(GetStatGrowthRange(stat, groupIndex).Range, GrowthStats.StatGrowthGroups[Stats.IsPromoted][groupIndex].Range.Range);
            return (DebugGrowthValues ? string.Format("{0:x}", growthValue) + " || " : "") +
                    GrowthStats.GetAverageStatGrowthPerLevelAsPercent(growthValue);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 200, displayGroup: "CurveCalc")] public string HPgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 201, displayGroup: "CurveCalc")] public string HPgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 202, displayGroup: "CurveCalc")] public string HPgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 203, displayGroup: "CurveCalc")] public string HPgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 204, displayGroup: "CurveCalc")] public string HPgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 205, displayGroup: "CurveCalc")] public string HPgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 210, displayGroup: "CurveCalc")] public string MPgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 211, displayGroup: "CurveCalc")] public string MPgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 212, displayGroup: "CurveCalc")] public string MPgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 213, displayGroup: "CurveCalc")] public string MPgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 214, displayGroup: "CurveCalc")] public string MPgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 215, displayGroup: "CurveCalc")] public string MPgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 220, displayGroup: "CurveCalc")] public string Atkgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 221, displayGroup: "CurveCalc")] public string Atkgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 222, displayGroup: "CurveCalc")] public string Atkgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 223, displayGroup: "CurveCalc")] public string Atkgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 224, displayGroup: "CurveCalc")] public string Atkgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 225, displayGroup: "CurveCalc")] public string Atkgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 230, displayGroup: "CurveCalc")] public string Defgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 231, displayGroup: "CurveCalc")] public string Defgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 232, displayGroup: "CurveCalc")] public string Defgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 233, displayGroup: "CurveCalc")] public string Defgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 234, displayGroup: "CurveCalc")] public string Defgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 235, displayGroup: "CurveCalc")] public string Defgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 240, displayGroup: "CurveCalc")] public string Agigroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 241, displayGroup: "CurveCalc")] public string Agigroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 242, displayGroup: "CurveCalc")] public string Agigroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 243, displayGroup: "CurveCalc")] public string Agigroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 244, displayGroup: "CurveCalc")] public string Agigroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 245, displayGroup: "CurveCalc")] public string Agigroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 5);
    }
}
