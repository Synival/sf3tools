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

        private static AI[] GetAIs(Slot slot) {
            return [
                new AI((byte) slot.AI1Tag, (byte) slot.AI1Type, (byte) slot.AI1Aggr),
                new AI((byte) slot.AI2Tag, (byte) slot.AI2Type, (byte) slot.AI2Aggr),
                new AI((byte) slot.AI3Tag, (byte) slot.AI3Type, (byte) slot.AI3Aggr),
                new AI((byte) slot.AI4Tag, (byte) slot.AI4Type, (byte) slot.AI4Aggr),
            ];
        }

        private static Cond[] GetConds(Slot slot) {
            return [
                new Cond((byte) slot.Cond1Zone, (byte) slot.Cond1Type, (byte) slot.Cond1AIIndex1, (byte) slot.Cond1AIIndex2),
                new Cond((byte) slot.Cond2Zone, (byte) slot.Cond2Type, (byte) slot.Cond2AIIndex1, (byte) slot.Cond2AIIndex2),
                new Cond((byte) slot.Cond3Zone, (byte) slot.Cond3Type, (byte) slot.Cond3AIIndex1, (byte) slot.Cond3AIIndex2),
                new Cond((byte) slot.Cond4Zone, (byte) slot.Cond4Type, (byte) slot.Cond4AIIndex1, (byte) slot.Cond4AIIndex2),
            ];
        }

        public static string[]? HasUnknownBattleFlag(IX1_File x1File) {
            return x1File.Battles?.Any() != true
                ? null
                : x1File.Battles.SelectMany(x => x.Value.SlotTable.Rows.Where(y => y.IgnoreConditions).Select(y => $"{x.Key}: 0x{y.ID:X2}")).ToArray();
        }

        private static string[]? AISearchBase(string filename, IX1_File x1File, Func<Battle, Slot, bool> pred) {
            if (x1File.Battles?.Any() != true)
                return null;

            return x1File.Battles
                .SelectMany(x => x.Value.SlotTable.Rows
                    .Where(y => y.ID < x.Value.BattleHeader.NumSlots && y.EnemyID != 0 && y.EnemyID < 0x1000 && pred(x.Value, y))
                    .Select(y => FormatAIRow(filename, x.Key, x.Value, y, x1File.NameGetterContext))
                )
                .ToArray();
        }

        public static string[]? HasCondType01(string filename, IX1_File x1File) {
            bool hasAnyCondFlag0100(Slot slot) {
                return
                    slot.Cond1Type == 0x01 ||
                    slot.Cond2Type == 0x01 ||
                    slot.Cond3Type == 0x01 ||
                    slot.Cond4Type == 0x01;
            }

            return AISearchBase(filename, x1File, (_, slot) => slot.DefaultAIIndex == 0xFF && hasAnyCondFlag0100(slot));
        }

        public static string[]? HasCondWithAnyNonZeroAIIndex1(string filename, IX1_File x1File) {
            bool hasAnyCondFlag0100(Slot slot) {
                return
                    (slot.Cond1Type != 0xFF && slot.Cond1AIIndex1 != 0) ||
                    (slot.Cond2Type != 0xFF && slot.Cond2AIIndex1 != 0) ||
                    (slot.Cond3Type != 0xFF && slot.Cond3AIIndex1 != 0) ||
                    (slot.Cond4Type != 0xFF && slot.Cond4AIIndex1 != 0);
            }

            return AISearchBase(filename, x1File, (_, slot) => slot.DefaultAIIndex == 0xFF && hasAnyCondFlag0100(slot));
        }

        public static string[]? HasOnlyCond2or3or4(string filename, IX1_File x1File) {
            bool condsCheck(Slot slot) {
                var conds = GetConds(slot);
                return !conds[0].Exists && conds.Skip(1).Any(x => x.Exists);
            }
            return AISearchBase(filename, x1File, (_, slot) => condsCheck(slot));
        }

        public static string[]? HasCond3or4ButNot2(string filename, IX1_File x1File) {
            bool condsCheck(Slot slot) {
                var conds = GetConds(slot);
                return !conds[1].Exists && (conds[2].Exists || conds[3].Exists);
            }
            return AISearchBase(filename, x1File, (_, slot) => condsCheck(slot));
        }

        public static string[]? HasCondZoneWith0x80(string filename, IX1_File x1File) {
            bool hasAnyCondFlag0100(Slot slot) {
                return
                    ((slot.Cond1Zone & 0xF0) == 0x80) ||
                    ((slot.Cond2Zone & 0xF0) == 0x80) ||
                    ((slot.Cond3Zone & 0xF0) == 0x80) ||
                    ((slot.Cond4Zone & 0xF0) == 0x80);
            }

            return AISearchBase(filename, x1File, (_, slot) => slot.DefaultAIIndex == 0xFF && hasAnyCondFlag0100(slot));
        }

        public static string[]? HasNoConditionsButHasAIs(string filename, IX1_File x1File) {
            bool MatchCond(Slot slot) {
                return (slot.Cond1Type == 0xFF && slot.Cond2Type == 0xFF && slot.Cond3Type == 0xFF && slot.Cond4Type == 0xFF) &&
                       (slot.AI1Type != 0xFF || slot.AI2Type != 0xFF && slot.AI3Type != 0xFF && slot.AI4Type != 0xFF);
            };

            return AISearchBase(filename, x1File, (_, slot) => slot.DefaultAIIndex == 0xFF && MatchCond(slot));
        }

        public static string[]? HasWeirdPath(string filename, IX1_File x1File) {
            bool MatchCond(Slot slot) {
                var ais = GetAIs(slot);
                return ais.Any(x => x.IsPath && x.Type != 0xF0 && x.Type != 0xF2);
            };
            return AISearchBase(filename, x1File, (_, slot) => MatchCond(slot));
        }

        public static string[]? HasCond1WithoutDefaultAI(string filename, IX1_File x1File) {
            bool MatchCond(Slot slot) {
                return slot.DefaultAIIndex != 0xFF && slot.Cond1Zone != 0xFF;
            };
            return AISearchBase(filename, x1File, (_, slot) => MatchCond(slot));
        }

        public static string[]? DumpBattleAIs(string filename, IX1_File x1File)
            => AISearchBase(filename, x1File, (_, _) => true);

        public static string[]? HasWeirdCondFlags(string filename, IX1_File x1File) {
            bool isWeirdFlags(int mvmt) => (mvmt != 0xFFFF && mvmt != 0x0000 && mvmt != 0x0100 && mvmt != 0x1000 && mvmt != 0x1100);
            bool hasWeirdFlags(Slot slot) => isWeirdFlags(slot.Cond1Type) || isWeirdFlags(slot.Cond2Type) || isWeirdFlags(slot.Cond3Type) || isWeirdFlags(slot.Cond4Type);
            return AISearchBase(filename, x1File, (_, slot) => hasWeirdFlags(slot));
        }

        public static string[]? HasWeirdCondZone(string filename, IX1_File x1File) {
            // NOTE: It looks like the zone can have the 0x80 bit set. What does it mean in that case...?
            bool isWeirdZone(Battle battle, int zone) => zone != 0xFF && (zone & 0x7F) >= battle.BattleHeader.NumZones;
            bool hasWeirdZone(Battle battle, Slot slot) {
                return isWeirdZone(battle, slot.Cond1Zone) ||
                       isWeirdZone(battle, slot.Cond2Zone) ||
                       isWeirdZone(battle, slot.Cond3Zone) ||
                       isWeirdZone(battle, slot.Cond4Zone);
            };
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? HasCond2PlusWithOffConditionBehavior(string filename, IX1_File x1File) {
            // Filter out the "always check" flag, because in this case, the fallback behavior DOES make sense.
            bool isWeirdZone(Battle battle, Cond cond) => cond.Type == 0x01 || cond.Type == 0x11 && ((cond.Zone & 0x80) == 0);
            bool hasWeirdZone(Battle battle, Slot slot) {
                var conds = GetConds(slot);
                return // ignore Cond1, because that one is a bit special.
                       isWeirdZone(battle, conds[1]) ||
                       isWeirdZone(battle, conds[2]) ||
                       isWeirdZone(battle, conds[3]);
            };
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? HasAlwaysCheckWithType00(string filename, IX1_File x1File) {
            bool isWeirdZone(Battle battle, Cond cond) => ((cond.Zone & 0x80) == 0x80 && cond.Type == 0x00);
            bool hasWeirdZone(Battle battle, Slot slot) {
                var conds = GetConds(slot);
                return conds.Any(x => isWeirdZone(battle, x));
            };
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? HasCond1With01Or11(string filename, IX1_File x1File) {
            bool hasWeirdZone(Battle battle, Slot slot) {
                var conds = GetConds(slot);
                return ((conds[0].Zone & 0x80) == 0x00) && (conds[0].Type == 0x01 || conds[0].Type == 0x11);
            };
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? Has0z00(string filename, IX1_File x1File) {
            bool hasWeirdZone(Battle battle, Slot slot) {
                var conds = GetConds(slot);
                return conds.Skip(1).Any(x => !x.AlwaysCheck && x.Type == 0x00);
            };
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? Has8z00(string filename, IX1_File x1File) {
            bool hasWeirdZone(Battle battle, Slot slot) {
                var conds = GetConds(slot);
                return conds.Skip(1).Any(x => x.AlwaysCheck && x.Type == 0x00);
            };
            return AISearchBase(filename, x1File, hasWeirdZone);
        }

        public static string[]? HasCond0AndMore(string filename, IX1_File x1File) {
            bool condsCheck(Slot slot) {
                var conds = GetConds(slot);
                return conds[0].Exists && conds.Skip(1).Any(x => x.Exists);
            }
            return AISearchBase(filename, x1File, (_, slot) => condsCheck(slot));
        }

        public static string[]? HasBattleOrFlagID(string filename, IX1_File x1File)
            => AISearchBase(filename, x1File, (battle, slot) => slot.EnemyID != 0x5F && slot.FlagOrBattleID != 0);

        public static string[]? HasVerySpecialAI(string filename, IX1_File x1File)
            => AISearchBase(filename, x1File, (battle, slot) => GetAIs(slot).Any(x => x.IsSpecial && x.Tag >= 0x02));

        public static string FormatAIRow(string filename, MapLeaderType leader, Battle battle, Slot slot, INameGetterContext ngc) {
            return
                $"{filename,-8}, {leader,-7}: " +
                $"{slot.ID:X02} - {slot.EnemyID:X02} ({ngc.GetName(null, null, slot.EnemyID, [NamedValueType.Monster]),-30}) - " +
                $"{slot.DefaultAIIndex:X02}, " +
                "AI:[" +
                  $"0:({slot.AI1Tag:X02}{slot.AI1Type:X02}{slot.AI1Aggr:X02}), " +
                  $"1:({slot.AI2Tag:X02}{slot.AI2Type:X02}{slot.AI2Aggr:X02}), " +
                  $"2:({slot.AI3Tag:X02}{slot.AI3Type:X02}{slot.AI3Aggr:X02}), " +
                  $"3:({slot.AI4Tag:X02}{slot.AI4Type:X02}{slot.AI4Aggr:X02})" +
                "], " +
                "Cond:[" +
                  $"0:({slot.Cond1Zone:X02}{slot.Cond1Type:X02}{slot.Cond1AIIndex1:X02}{slot.Cond1AIIndex2:X02}), " +
                  $"1:({slot.Cond2Zone:X02}{slot.Cond2Type:X02}{slot.Cond2AIIndex1:X02}{slot.Cond2AIIndex2:X02}), " +
                  $"2:({slot.Cond3Zone:X02}{slot.Cond3Type:X02}{slot.Cond3AIIndex1:X02}{slot.Cond3AIIndex2:X02}), " +
                  $"3:({slot.Cond4Zone:X02}{slot.Cond4Type:X02}{slot.Cond4AIIndex1:X02}{slot.Cond4AIIndex2:X02})" +
                "], " +
                $"{slot.FlagOrBattleID:X2}";
        }
    }
}
