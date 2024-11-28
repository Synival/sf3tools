using System.Collections.Generic;
using SF3.Models.Files;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.X1;
using SF3.Models.Tables.X1.Battle;
using SF3.Models.Tables.X1.Town;
using SF3.Types;

namespace SF3.Models.Files.X1 {
    public interface IX1_Editor : IScenarioTableEditor {
        bool IsBTL99 { get; }

        bool IsBattle { get; }

        TreasureTable TreasureTable { get; }
        WarpTable WarpTable { get; }
        BattlePointersTable BattlePointersTable { get; }
        NpcTable NpcTable { get; }
        EnterTable EnterTable { get; }
        ArrowTable ArrowTable { get; }

        Dictionary<MapLeaderType, BattleEditor> Battles { get; }

        TileMovementTable TileMovementTable { get; }
    }
}
