using System.Collections.Generic;
using SF3.Tables.Shared;
using SF3.Tables.X1_All;
using SF3.Tables.X1_Battle;
using SF3.Tables.X1_Town;
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

        Dictionary<MapLeaderType, BattleTable> BattleTables { get; }

        TileMovementTable TileMovementTable { get; }
    }
}
