using System.Collections.Generic;
using SF3.Tables;
using SF3.Types;

namespace SF3.FileEditors {
    public interface IX1_FileEditor : ISF3FileEditor {
        bool IsBTL99 { get; }

        bool? IsBattle { get; }

        TreasureTable TreasureTable { get; }
        WarpTable WarpTable { get; }
        BattlePointersTable BattlePointersTable { get; }
        NpcTable NpcTable { get; }
        EnterTable EnterTable { get; }
        ArrowTable ArrowTable { get; }

        // TODO: temporary until ^^^!
        HeaderTable HeaderTable { get; }
        SlotTable SlotTable { get; }
        SpawnZoneTable SpawnZoneTable { get; }
        AITable AITable { get; }
        CustomMovementTable CustomMovementTable { get; }
        // ^^^^

        List<BattleTable> BattleTables { get; }

        TileMovementTable TileMovementTable { get; }
    }
}
