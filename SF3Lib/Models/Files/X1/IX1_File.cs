using System.Collections.Generic;
using CommonLib.Discovery;
using SF3.Models.Structs.Shared;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.X1;
using SF3.Models.Tables.X1.Battle;
using SF3.Models.Tables.X1.Town;
using SF3.Types;

namespace SF3.Models.Files.X1 {
    public interface IX1_File : IScenarioTableFile {
        uint RamAddress { get; }
        bool IsBTL99 { get; }
        bool IsBattle { get; }

        InteractableTable InteractableTable { get; }
        WarpTable WarpTable { get; }
        BattlePointersTable BattlePointersTable { get; }
        NpcTable NpcTable { get; }
        EnterTable EnterTable { get; }
        ArrowTable ArrowTable { get; }

        Dictionary<MapLeaderType, Battle> Battles { get; }

        TileMovementTable TileMovementTable { get; }
        CharacterTargetPriorityTable[] CharacterTargetPriorityTables { get; }
        CharacterTargetUnknownTable[] CharacterTargetUnknownTables { get; }
        Dictionary<uint, ModelInstanceGroupTable> ModelInstanceGroupTablesByAddress { get; }
        Dictionary<uint, ModelInstanceTable> ModelInstanceTablesByAddress { get; }
        Dictionary<uint, ActorScript> ScriptsByAddress { get; }
        MapUpdateFuncTable MapUpdateFuncTable { get; }
        BlacksmithTable BlacksmithTable { get; }

        DiscoveryContext Discoveries { get; }
    }
}
