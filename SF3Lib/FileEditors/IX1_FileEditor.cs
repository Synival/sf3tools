using SF3.Tables;
using SF3.Types;

namespace SF3.FileEditors {
    public interface IX1_FileEditor : ISF3FileEditor {
        // TODO: make this read-only once all of X1 is migrated. I suppose LoadTables() can set it privately
        MapLeaderType MapLeader { get; set; }

        // TODO: remove me once we have a table of tables for battles!!
        int MapIndex { get; }

        // TODO: remove me once all of X1 is migrated!!
        int MapOffset { get; }

        bool IsBattle { get; }

        bool IsBTL99 { get; }

        SlotTable SlotTable { get; }
        HeaderTable HeaderTable { get; }
        AITable AITable { get; }
        SpawnZoneTable SpawnZoneTable { get; }
        BattlePointersTable BattlePointersTable { get; }
        TreasureTable TreasureTable { get; }
        CustomMovementTable CustomMovementTable { get; }
        WarpTable WarpTable { get; }
        TileMovementTable TileMovementTable { get; }
        NpcTable NpcTable { get; }
        EnterTable EnterTable { get; }
        ArrowTable ArrowTable { get; }
    }
}
