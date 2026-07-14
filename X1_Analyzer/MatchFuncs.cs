using CommonLib.NamedValues;
using SF3.Models.Files.X1;
using SF3.Models.Structs.X1.Battle;
using SF3.Types;

namespace X1_Analyzer {
    public static class MatchFuncs {
        public static string[]? HasUnknownBattleFlag(IX1_File x1File) {
            var battles = x1File.GetBattleMaps().Values.ToArray();
            if (battles.Length == 0)
                return null;

            return battles.SelectMany(x => x.UnitTable.Rows.Where(y => y.IgnoreConditions).Select(y => $"{x.MapLeader}: 0x{y.ID:X2}")).ToArray();
        }

        private static string[]? AISearchBase(string filename, IX1_File x1File, Func<BattleMap, Unit, bool> pred) {
            var battles = x1File.GetBattleMaps().Values.ToArray();
            if (battles.Length == 0)
                return null;

            return battles
                .SelectMany(x => x.UnitTable.Rows
                    .Where(y => y.ID < x.Header.NumUnits && y.EnemyID != 0 && y.EnemyID < 0x1000 && pred(x, y))
                    .Select(y => FormatAIRow(filename, x.MapLeader, x, y, x1File.NameGetterContext))
                )
                .ToArray();
        }

        public static string[]? HasCondType01(string filename, IX1_File x1File) {
            bool hasAnyCondFlag0100(Unit unit) => unit.Conditions.Any(x => x.OrderFlags == 0x01);
            return AISearchBase(filename, x1File, (_, unit) => unit.DefaultAIIndex == 0xFF && hasAnyCondFlag0100(unit));
        }

        public static string[]? HasCondWithAnyNonZeroAIIndex1(string filename, IX1_File x1File) {
            bool hasAnyCondFlag0100(Unit unit) => unit.Conditions.Any(x => x.OrderFlags != 0xFF && x.OffAIIndex != 0);
            return AISearchBase(filename, x1File, (_, unit) => unit.DefaultAIIndex == 0xFF && hasAnyCondFlag0100(unit));
        }

        public static string[]? HasOnlyCond2or3or4(string filename, IX1_File x1File) {
            bool condsCheck(Unit unit) => !unit.Conditions[0].Exists && unit.Conditions.Skip(1).Any(x => x.Exists);
            return AISearchBase(filename, x1File, (_, unit) => condsCheck(unit));
        }

        public static string[]? HasCond3or4ButNot2(string filename, IX1_File x1File) {
            bool condsCheck(Unit unit) => !unit.Conditions[1].Exists && (unit.Conditions[2].Exists || unit.Conditions[3].Exists);
            return AISearchBase(filename, x1File, (_, unit) => condsCheck(unit));
        }

        public static string[]? HasCondZoneWith0x80(string filename, IX1_File x1File) {
            bool hasAnyCondFlag0100(Unit unit) => unit.Conditions.Any(x => (x.Zone & 0xF0) == 0x80);
            return AISearchBase(filename, x1File, (_, unit) => unit.DefaultAIIndex == 0xFF && hasAnyCondFlag0100(unit));
        }

        public static string[]? HasNoConditionsButHasAIs(string filename, IX1_File x1File) {
            bool MatchCond(Unit unit) => unit.Conditions.All(x => !x.Exists) && unit.Orders.Any(x => x.Exists);
            return AISearchBase(filename, x1File, (_, unit) => unit.DefaultAIIndex == 0xFF && MatchCond(unit));
        }

        public static string[]? HasWeirdPath(string filename, IX1_File x1File) {
            bool MatchCond(Unit unit) => unit.Orders.Any(x => x.Type == AIOrderType.Path && x.TargetFlags != 0xF0 && x.TargetFlags != 0xF2);
            return AISearchBase(filename, x1File, (_, unit) => MatchCond(unit));
        }

        public static string[]? HasCond1WithoutDefaultAI(string filename, IX1_File x1File) {
            bool MatchCond(Unit unit) => unit.DefaultAIIndex != 0xFF && unit.Conditions[0].Exists;
            return AISearchBase(filename, x1File, (_, unit) => MatchCond(unit));
        }

        public static string[]? DumpBattleAIs(string filename, IX1_File x1File)
            => AISearchBase(filename, x1File, (_, _) => true);

        public static string[]? HasWeirdCondFlags(string filename, IX1_File x1File) {
            bool isWeirdFlags(int mvmt) => (mvmt != 0xFFFF && mvmt != 0x0000 && mvmt != 0x0100 && mvmt != 0x1000 && mvmt != 0x1100);
            bool hasWeirdFlags(Unit unit) => unit.Conditions.Any(x => isWeirdFlags(x.OrderFlags));
            return AISearchBase(filename, x1File, (_, unit) => hasWeirdFlags(unit));
        }

        public static string[]? HasWeirdCondZone(string filename, IX1_File x1File) {
            // NOTE: It looks like the zone can have the 0x80 bit set. What does it mean in that case...?
            bool isWeirdZone(BattleMap battle, int zone) => zone != 0xFF && (zone & 0x7F) >= battle.Header.NumZones;
            bool hasWeirdZone(BattleMap battle, Unit unit) => unit.Conditions.Any(x => isWeirdZone(battle, x.Zone));
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? HasCond2PlusWithOffConditionBehavior(string filename, IX1_File x1File) {
            // Filter out the "always check" flag, because in this case, the fallback behavior DOES make sense.
            bool isWeirdZone(BattleMap battle, UnitAICondition cond) => cond.OrderFlags == 0x01 || cond.OrderFlags == 0x11 && ((cond.Zone & 0x80) == 0);
            bool hasWeirdZone(BattleMap battle, Unit unit) {
                // ignore Cond1, because that one is a bit special.
                return unit.Conditions.Skip(1).Any(x => isWeirdZone(battle, x));
            };
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? HasAlwaysCheckWithType00(string filename, IX1_File x1File) {
            bool isWeirdZone(BattleMap battle, UnitAICondition cond) => (cond.Zone & 0x80) == 0x80 && cond.OrderFlags == 0x00;
            bool hasWeirdZone(BattleMap battle, Unit unit) => unit.Conditions.Any(x => isWeirdZone(battle, x));
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? HasCond1With01Or11(string filename, IX1_File x1File) {
            bool hasWeirdZone(BattleMap battle, Unit unit) {
                var cond = unit.Conditions[0];
                return (cond.Zone & 0x80) == 0x00 && (cond.OrderFlags == 0x01 || cond.OrderFlags == 0x11);
            }
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? Has0z00(string filename, IX1_File x1File) {
            bool hasWeirdZone(BattleMap battle, Unit unit) => unit.Conditions.Skip(1).Any(x => !x.AlwaysCheck && x.OrderFlags == 0x00);
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? Has8z00(string filename, IX1_File x1File) {
            bool hasWeirdZone(BattleMap battle, Unit unit) => unit.Conditions.Skip(1).Any(x => x.AlwaysCheck && x.OrderFlags == 0x00);
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? HasCond0AndMore(string filename, IX1_File x1File) {
            bool condsCheck(Unit unit) => unit.Conditions[0].Exists && unit.Conditions.Skip(1).Any(x => x.Exists);
            return AISearchBase(filename, x1File, (_, unit) => condsCheck(unit));
        }

        public static string[]? HasBattleOrFlagID(string filename, IX1_File x1File)
            => AISearchBase(filename, x1File, (battle, unit) => unit.EnemyID != 0x5F && unit.FlagOrUnitID != 0);

        public static string[]? HasInvaildAI(string filename, IX1_File x1File)
            => AISearchBase(filename, x1File, (battle, unit) => unit.Orders.Any(x => x.Type == AIOrderType.Invalid));

        public static string FormatAIRow(string filename, MapLeaderType leader, BattleMap battle, Unit unit, INameGetterContext ngc) {
            return
                $"{filename,-8}, {leader,-7}: " +
                $"{unit.ID:X02} - {unit.EnemyID:X02} ({ngc.GetName(null, null, unit.EnemyID, [NamedValueType.Monster]),-30}) - " +
                $"{unit.DefaultAIIndex:X02}, " +
                "Orders:[" + string.Join(",", unit.Orders.Select((x, i) => $"{i}:({x.Target:X02}{x.TargetFlags:X02}{x.Aggression:X02})")) + "], " +
                "Conds:[" + string.Join(",", unit.Conditions.Select((x, i) => $"{i}:({x.Zone:X02}{x.OrderFlags:X02}{x.OffAIIndex:X02}{x.OnAIIndex:X02})")) + "], " +
                $"{unit.FlagOrUnitID:X2}";
        }
    }
}
