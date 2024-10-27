using SF3.Tables;
using SF3.Tables.X1;
using SF3.Types;

namespace SF3.FileEditors {
    public interface IX1_FileEditor : ISF3FileEditor {
        // TODO: This really should be read-only, but some of the models set this. How to fix?
        MapLeaderType MapLeader { get; set; }

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
