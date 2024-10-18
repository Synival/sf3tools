using SF3.FileEditors;
using SF3.Types;
using SF3.X1_Editor.Models.AI;
using SF3.X1_Editor.Models.Arrows;
using SF3.X1_Editor.Models.BattlePointers;
using SF3.X1_Editor.Models.CustomMovement;
using SF3.X1_Editor.Models.Enters;
using SF3.X1_Editor.Models.Headers;
using SF3.X1_Editor.Models.Npcs;
using SF3.X1_Editor.Models.Slots;
using SF3.X1_Editor.Models.SpawnZones;
using SF3.X1_Editor.Models.Tiles;
using SF3.X1_Editor.Models.Treasures;
using SF3.X1_Editor.Models.Warps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.X1_Editor.FileEditors
{
    public interface IX1_FileEditor : ISF3FileEditor
    {
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
