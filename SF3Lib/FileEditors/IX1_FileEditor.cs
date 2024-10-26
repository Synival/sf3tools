using SF3.Tables.X1.AI;
using SF3.Tables.X1.Arrow;
using SF3.Tables.X1.BattlePointer;
using SF3.Tables.X1.CustomMovement;
using SF3.Tables.X1.Enter;
using SF3.Tables.X1.Header;
using SF3.Tables.X1.Npc;
using SF3.Tables.X1.Slot;
using SF3.Tables.X1.SpawnZone;
using SF3.Tables.X1.TileMovement;
using SF3.Tables.X1.Treasure;
using SF3.Tables.X1.Warp;
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
        TileMovementList TileList { get; }
        NpcList NpcList { get; }
        EnterList EnterList { get; }
        ArrowList ArrowList { get; }
    }
}
