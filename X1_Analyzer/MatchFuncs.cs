using CommonLib.NamedValues;
using SF3.Models.Files.X1;
using SF3.Models.Structs.X1.Battle;
using SF3.Types;

namespace X1_Analyzer {
    public static class MatchFuncs {
        private struct AI(byte tag, byte type, byte aggr) {
            public byte Tag = tag, Type = type, Aggr = aggr;

            public readonly bool IsSpecial  => Tag <= 0x31;
            public readonly bool IsLocation => Tag >= 0x32 && Tag <= 0x51;
            public readonly bool IsEnemy    => Tag >= 0x80 && Tag <= 0xBF;
            public readonly bool IsPath     => Tag >= 0xC0 && Tag <= 0xDF;
            public readonly bool IsUnknown  => Tag >= 0xF0 && Tag <= 0xFE;
            public readonly bool IsDefault  => Tag == 0xFF;
        }

        private struct Cond(byte zone, byte type, byte aIIndex1, byte aIIndex2) {
            public byte Zone = zone, Type = type, AIIndex1 = aIIndex1, AIIndex2 = aIIndex2;
            public readonly bool Exists => Zone != 0xFF;
            public readonly bool AlwaysCheck => (Zone & 0xF0) == 0x80;
        }

        private static AI[] GetAIs(Unit unit) {
            return [
                new AI((byte) unit.AI1Tag, (byte) unit.AI1Type, (byte) unit.AI1Aggr),
                new AI((byte) unit.AI2Tag, (byte) unit.AI2Type, (byte) unit.AI2Aggr),
                new AI((byte) unit.AI3Tag, (byte) unit.AI3Type, (byte) unit.AI3Aggr),
                new AI((byte) unit.AI4Tag, (byte) unit.AI4Type, (byte) unit.AI4Aggr),
            ];
        }

        private static Cond[] GetConds(Unit unit) {
            return [
                new Cond((byte) unit.Cond1Zone, (byte) unit.Cond1Type, (byte) unit.Cond1AIIndex1, (byte) unit.Cond1AIIndex2),
                new Cond((byte) unit.Cond2Zone, (byte) unit.Cond2Type, (byte) unit.Cond2AIIndex1, (byte) unit.Cond2AIIndex2),
                new Cond((byte) unit.Cond3Zone, (byte) unit.Cond3Type, (byte) unit.Cond3AIIndex1, (byte) unit.Cond3AIIndex2),
                new Cond((byte) unit.Cond4Zone, (byte) unit.Cond4Type, (byte) unit.Cond4AIIndex1, (byte) unit.Cond4AIIndex2),
            ];
        }

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
            bool hasAnyCondFlag0100(Unit unit) {
                return
                    unit.Cond1Type == 0x01 ||
                    unit.Cond2Type == 0x01 ||
                    unit.Cond3Type == 0x01 ||
                    unit.Cond4Type == 0x01;
            }

            return AISearchBase(filename, x1File, (_, unit) => unit.DefaultAIIndex == 0xFF && hasAnyCondFlag0100(unit));
        }

        public static string[]? HasCondWithAnyNonZeroAIIndex1(string filename, IX1_File x1File) {
            bool hasAnyCondFlag0100(Unit unit) {
                return
                    (unit.Cond1Type != 0xFF && unit.Cond1AIIndex1 != 0) ||
                    (unit.Cond2Type != 0xFF && unit.Cond2AIIndex1 != 0) ||
                    (unit.Cond3Type != 0xFF && unit.Cond3AIIndex1 != 0) ||
                    (unit.Cond4Type != 0xFF && unit.Cond4AIIndex1 != 0);
            }

            return AISearchBase(filename, x1File, (_, unit) => unit.DefaultAIIndex == 0xFF && hasAnyCondFlag0100(unit));
        }

        public static string[]? HasOnlyCond2or3or4(string filename, IX1_File x1File) {
            bool condsCheck(Unit unit) {
                var conds = GetConds(unit);
                return !conds[0].Exists && conds.Skip(1).Any(x => x.Exists);
            }
            return AISearchBase(filename, x1File, (_, unit) => condsCheck(unit));
        }

        public static string[]? HasCond3or4ButNot2(string filename, IX1_File x1File) {
            bool condsCheck(Unit unit) {
                var conds = GetConds(unit);
                return !conds[1].Exists && (conds[2].Exists || conds[3].Exists);
            }
            return AISearchBase(filename, x1File, (_, unit) => condsCheck(unit));
        }

        public static string[]? HasCondZoneWith0x80(string filename, IX1_File x1File) {
            bool hasAnyCondFlag0100(Unit unit) {
                return
                    ((unit.Cond1Zone & 0xF0) == 0x80) ||
                    ((unit.Cond2Zone & 0xF0) == 0x80) ||
                    ((unit.Cond3Zone & 0xF0) == 0x80) ||
                    ((unit.Cond4Zone & 0xF0) == 0x80);
            }

            return AISearchBase(filename, x1File, (_, unit) => unit.DefaultAIIndex == 0xFF && hasAnyCondFlag0100(unit));
        }

        public static string[]? HasNoConditionsButHasAIs(string filename, IX1_File x1File) {
            bool MatchCond(Unit unit) {
                return (unit.Cond1Type == 0xFF && unit.Cond2Type == 0xFF && unit.Cond3Type == 0xFF && unit.Cond4Type == 0xFF) &&
                       (unit.AI1Type != 0xFF || unit.AI2Type != 0xFF && unit.AI3Type != 0xFF && unit.AI4Type != 0xFF);
            };

            return AISearchBase(filename, x1File, (_, unit) => unit.DefaultAIIndex == 0xFF && MatchCond(unit));
        }

        public static string[]? HasWeirdPath(string filename, IX1_File x1File) {
            bool MatchCond(Unit unit) {
                var ais = GetAIs(unit);
                return ais.Any(x => x.IsPath && x.Type != 0xF0 && x.Type != 0xF2);
            };
            return AISearchBase(filename, x1File, (_, unit) => MatchCond(unit));
        }

        public static string[]? HasCond1WithoutDefaultAI(string filename, IX1_File x1File) {
            bool MatchCond(Unit unit) {
                return unit.DefaultAIIndex != 0xFF && unit.Cond1Zone != 0xFF;
            };
            return AISearchBase(filename, x1File, (_, unit) => MatchCond(unit));
        }

        public static string[]? DumpBattleAIs(string filename, IX1_File x1File)
            => AISearchBase(filename, x1File, (_, _) => true);

        public static string[]? HasWeirdCondFlags(string filename, IX1_File x1File) {
            bool isWeirdFlags(int mvmt) => (mvmt != 0xFFFF && mvmt != 0x0000 && mvmt != 0x0100 && mvmt != 0x1000 && mvmt != 0x1100);
            bool hasWeirdFlags(Unit unit) => isWeirdFlags(unit.Cond1Type) || isWeirdFlags(unit.Cond2Type) || isWeirdFlags(unit.Cond3Type) || isWeirdFlags(unit.Cond4Type);
            return AISearchBase(filename, x1File, (_, unit) => hasWeirdFlags(unit));
        }

        public static string[]? HasWeirdCondZone(string filename, IX1_File x1File) {
            // NOTE: It looks like the zone can have the 0x80 bit set. What does it mean in that case...?
            bool isWeirdZone(BattleMap battle, int zone) => zone != 0xFF && (zone & 0x7F) >= battle.Header.NumZones;
            bool hasWeirdZone(BattleMap battle, Unit unit) {
                return isWeirdZone(battle, unit.Cond1Zone) ||
                       isWeirdZone(battle, unit.Cond2Zone) ||
                       isWeirdZone(battle, unit.Cond3Zone) ||
                       isWeirdZone(battle, unit.Cond4Zone);
            };
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? HasCond2PlusWithOffConditionBehavior(string filename, IX1_File x1File) {
            // Filter out the "always check" flag, because in this case, the fallback behavior DOES make sense.
            bool isWeirdZone(BattleMap battle, Cond cond) => cond.Type == 0x01 || cond.Type == 0x11 && ((cond.Zone & 0x80) == 0);
            bool hasWeirdZone(BattleMap battle, Unit unit) {
                var conds = GetConds(unit);
                return // ignore Cond1, because that one is a bit special.
                       isWeirdZone(battle, conds[1]) ||
                       isWeirdZone(battle, conds[2]) ||
                       isWeirdZone(battle, conds[3]);
            };
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? HasAlwaysCheckWithType00(string filename, IX1_File x1File) {
            bool isWeirdZone(BattleMap battle, Cond cond) => ((cond.Zone & 0x80) == 0x80 && cond.Type == 0x00);
            bool hasWeirdZone(BattleMap battle, Unit unit) {
                var conds = GetConds(unit);
                return conds.Any(x => isWeirdZone(battle, x));
            };
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? HasCond1With01Or11(string filename, IX1_File x1File) {
            bool hasWeirdZone(BattleMap battle, Unit unit) {
                var conds = GetConds(unit);
                return ((conds[0].Zone & 0x80) == 0x00) && (conds[0].Type == 0x01 || conds[0].Type == 0x11);
            };
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? Has0z00(string filename, IX1_File x1File) {
            bool hasWeirdZone(BattleMap battle, Unit unit) {
                var conds = GetConds(unit);
                return conds.Skip(1).Any(x => !x.AlwaysCheck && x.Type == 0x00);
            };
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? Has8z00(string filename, IX1_File x1File) {
            bool hasWeirdZone(BattleMap battle, Unit unit) {
                var conds = GetConds(unit);
                return conds.Skip(1).Any(x => x.AlwaysCheck && x.Type == 0x00);
            };
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? HasCond0AndMore(string filename, IX1_File x1File) {
            bool condsCheck(Unit unit) {
                var conds = GetConds(unit);
                return conds[0].Exists && conds.Skip(1).Any(x => x.Exists);
            }
            return AISearchBase(filename, x1File, (_, unit) => condsCheck(unit));
        }

        public static string[]? HasBattleOrFlagID(string filename, IX1_File x1File)
            => AISearchBase(filename, x1File, (battle, unit) => unit.EnemyID != 0x5F && unit.FlagOrBattleID != 0);

        public static string[]? HasVerySpecialAI(string filename, IX1_File x1File)
            => AISearchBase(filename, x1File, (battle, unit) => GetAIs(unit).Any(x => x.IsSpecial && x.Tag >= 0x02));

        public static string FormatAIRow(string filename, MapLeaderType leader, BattleMap battle, Unit unit, INameGetterContext ngc) {
            return
                $"{filename,-8}, {leader,-7}: " +
                $"{unit.ID:X02} - {unit.EnemyID:X02} ({ngc.GetName(null, null, unit.EnemyID, [NamedValueType.Monster]),-30}) - " +
                $"{unit.DefaultAIIndex:X02}, " +
                "AI:[" +
                  $"0:({unit.AI1Tag:X02}{unit.AI1Type:X02}{unit.AI1Aggr:X02}), " +
                  $"1:({unit.AI2Tag:X02}{unit.AI2Type:X02}{unit.AI2Aggr:X02}), " +
                  $"2:({unit.AI3Tag:X02}{unit.AI3Type:X02}{unit.AI3Aggr:X02}), " +
                  $"3:({unit.AI4Tag:X02}{unit.AI4Type:X02}{unit.AI4Aggr:X02})" +
                "], " +
                "Cond:[" +
                  $"0:({unit.Cond1Zone:X02}{unit.Cond1Type:X02}{unit.Cond1AIIndex1:X02}{unit.Cond1AIIndex2:X02}), " +
                  $"1:({unit.Cond2Zone:X02}{unit.Cond2Type:X02}{unit.Cond2AIIndex1:X02}{unit.Cond2AIIndex2:X02}), " +
                  $"2:({unit.Cond3Zone:X02}{unit.Cond3Type:X02}{unit.Cond3AIIndex1:X02}{unit.Cond3AIIndex2:X02}), " +
                  $"3:({unit.Cond4Zone:X02}{unit.Cond4Type:X02}{unit.Cond4AIIndex1:X02}{unit.Cond4AIIndex2:X02})" +
                "], " +
                $"{unit.FlagOrBattleID:X2}";
        }
    }
}
