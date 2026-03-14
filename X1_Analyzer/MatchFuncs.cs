using CommonLib.NamedValues;
using SF3.Models.Files.X1;
using SF3.Models.Structs.X1.Battle;
using SF3.Types;

namespace X1_Analyzer {
    public static class MatchFuncs {
        public static string[]? HasUnknownBattleFlag(IX1_File x1File) {
            return x1File.Battles?.Any() != true
                ? null
                : x1File.Battles.SelectMany(x => x.Value.SlotTable.Rows.Where(y => y.UnknownFlag).Select(y => $"{x.Key}: 0x{y.ID:X2}")).ToArray();
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

        public static string[]? HasWeirdCreepUpFlag(string filename, IX1_File x1File)
            => AISearchBase(filename, x1File, (_, slot) => slot.DefaultAIIndex != 0x00 && slot.DefaultAIIndex != 0xFF);

        public static string[]? HasCreepUpFlagButAlsoAI(string filename, IX1_File x1File) {
            bool hasAnyNonDefaultAI(Slot slot) {
                return
                    (slot.AI1Tag != 0xFF || slot.AI1Type != 0xFF || slot.AI1Aggr != 0x00) ||
                    (slot.AI2Tag != 0xFF || slot.AI2Type != 0xFF || slot.AI2Aggr != 0x00) ||
                    (slot.AI3Tag != 0xFF || slot.AI3Type != 0xFF || slot.AI3Aggr != 0x00) ||
                    (slot.AI4Tag != 0xFF || slot.AI4Type != 0xFF || slot.AI4Aggr != 0x00);
            }

            return AISearchBase(filename, x1File, (_, slot) => slot.DefaultAIIndex == 0xFF && hasAnyNonDefaultAI(slot));
        }

        public static string[]? DumpBattleAIs(string filename, IX1_File x1File)
            => AISearchBase(filename, x1File, (_, _) => true);

        public static string[]? HasWeirdCondFlags(string filename, IX1_File x1File) {
            bool isWeirdFlags(int mvmt) => (mvmt != 0xFFFF && mvmt != 0x0000 && mvmt != 0x0100 && mvmt != 0x1000 && mvmt != 0x1100);
            bool hasWeirdFlags(Slot slot) => isWeirdFlags(slot.Cond1Flags) || isWeirdFlags(slot.Cond2Flags) || isWeirdFlags(slot.Cond3Flags) || isWeirdFlags(slot.Cond4Flags);
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

        public static string FormatAIRow(string filename, MapLeaderType leader, Battle battle, Slot slot, INameGetterContext ngc) {
            return
                $"{filename,-8}, {leader,-7}: " +
                $"{slot.ID:X02} - {slot.EnemyID:X02} ({ngc.GetName(null, null, slot.EnemyID, [NamedValueType.Monster]),-20}) - " +
                $"{slot.DefaultAIIndex:X02}, " +
                "AI:[" +
                  $"1:({slot.AI1Tag:X02},{slot.AI1Type:X02},{slot.AI1Aggr:X02}), " +
                  $"2:({slot.AI2Tag:X02},{slot.AI2Type:X02},{slot.AI2Aggr:X02}), " +
                  $"3:({slot.AI3Tag:X02},{slot.AI3Type:X02},{slot.AI3Aggr:X02}), " +
                  $"4:({slot.AI4Tag:X02},{slot.AI4Type:X02},{slot.AI4Aggr:X02})" +
                "], " +
                "Cond:[" +
                  $"1:({slot.Cond1Zone:X02},{slot.Cond1Flags:X04},{slot.Cond1AIIndex:X02}), " +
                  $"2:({slot.Cond2Zone:X02},{slot.Cond2Flags:X04},{slot.Cond2AIIndex:X02}), " +
                  $"3:({slot.Cond3Zone:X02},{slot.Cond3Flags:X04},{slot.Cond3AIIndex:X02}), " +
                  $"4:({slot.Cond4Zone:X02},{slot.Cond4Flags:X04},{slot.Cond4AIIndex:X02})" +
                "]";
        }
    }
}
