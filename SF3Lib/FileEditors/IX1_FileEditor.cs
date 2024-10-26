using SF3.Models.X1.AI;
using SF3.Models.X1.Arrow;
using SF3.Models.X1.BattlePointers;
using SF3.Models.X1.CustomMovement;
using SF3.Models.X1.Enter;
using SF3.Models.X1.Headers;
using SF3.Models.X1.Npc;
using SF3.Models.X1.Slots;
using SF3.Models.X1.SpawnZones;
using SF3.Models.X1.TileMovement;
using SF3.Models.X1.Treasure;
using SF3.Models.X1.Warp;
using SF3.Types;

namespace SF3.FileEditors {
    public interface IX1_FileEditor : ISF3FileEditor {
        // TODO: This really should be read-only, but some of the models set this. How to fix?
        MapLeaderType MapLeader { get; set; }

        int MapOffset { get; }

        bool IsBattle { get; }

        bool IsBTL99 { get; }

        SlotList SlotList { get; }
        HeaderList HeaderList { get; }
        AIList AIList { get; }
        SpawnZoneList SpawnZoneList { get; }
        BattlePointersList BattlePointersList { get; }
        TreasureList TreasureList { get; }
        CustomMovementList CustomMovementList { get; }
        WarpList WarpList { get; }
        TileList TileList { get; }
        NpcList NpcList { get; }
        EnterList EnterList { get; }
        ArrowList ArrowList { get; }
    }
}
