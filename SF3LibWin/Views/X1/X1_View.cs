﻿using System.Linq;
using System.Windows.Forms;
using CommonLib.Types;
using SF3.Models.Files.X1;
using SF3.Models.Tables.X1;

namespace SF3.Win.Views.X1 {
    public class X1_View : TabView {
        public X1_View(string name, IX1_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.InteractableTable != null)
                CreateChild(new TableView("Interactables", Model.InteractableTable, ngc));
            if (Model.BattlePointersTable != null)
                CreateChild(new TableView("Battle Pointers", Model.BattlePointersTable, ngc));
            if (Model.NpcTable != null)
                CreateChild(new TableView("Town NPCs", Model.NpcTable, ngc));
            if (Model.EnterTable != null)
                CreateChild(new TableView("Non-Battle Enter", Model.EnterTable, ngc));
            if (Model.WarpTable != null)
                CreateChild(new TableView("Warp Table (Scn2+)", Model.WarpTable, ngc));
            if (Model.ArrowTable != null)
                CreateChild(new TableView("Arrows (Scn2+)", Model.ArrowTable, ngc));
            if (Model.TileMovementTable != null)
                CreateChild(new TableView("Tile Data (Scn2+)", Model.TileMovementTable, ngc));
            if (Model.CharacterTargetPriorityTables != null)
                CreateChild(new TableArrayView<CharacterTargetPriorityTable>("Character Target Priorities", Model.CharacterTargetPriorityTables, ngc));
            if (Model.CharacterTargetUnknownTables != null)
                CreateChild(new TableArrayView<CharacterTargetUnknownTable>("Unknown 16 Tables", Model.CharacterTargetUnknownTables, ngc));
            if (Model.ModelInstanceGroupTablesByAddress?.Count > 0)
                CreateChild(new TableArrayView<ModelInstanceGroupTable>("Model Instance Groups", Model.ModelInstanceGroupTablesByAddress.Values.ToArray(), ngc));
            if (Model.ModelInstanceTablesByAddress?.Count > 0)
                CreateChild(new TableArrayView<ModelInstanceTable>("Model Instances", Model.ModelInstanceTablesByAddress.Values.ToArray(), ngc));
            if (Model.MapUpdateFuncTable != null)
                CreateChild(new TableView("Map Update Functions", Model.MapUpdateFuncTable, ngc));

            if (Model.ScriptsByAddress?.Count > 0) {
                CreateChild(new TextArrayView("Scripts",
                    Model.ScriptsByAddress.ToDictionary(
                        x =>
                            $"0x{x.Key.ToString("X8")}" +
                            (Model.ScriptsByAddress.ContainsKey(x.Key) ? $": {(Model.ScriptsByAddress[x.Key].ScriptName == "" ? "(unnamed)" : Model.ScriptsByAddress[x.Key].ScriptName)}" : ""),
                        x =>
                            "// " + (x.Value.ScriptName == "" ? "(unnamed)" : x.Value.ScriptName) + "\r\n" +
                            "// -------------------------------------------\r\n" +
                            string.Join("\r\n", x.Value.ScriptNote.Split("\r\n").Where(y => y != "").Select(y => "// " + y)) +
                            "\r\n\r\n" +
                            x.Value.Text
                    )
                ));
            }

            if (Model.Battles != null) {
                foreach (var battleKv in Model.Battles.Where(x => x.Value != null))
                    CreateChild(new BattleView($"Battle ({battleKv.Key})", battleKv.Value));
            }

            if (Model.Discoveries?.HasDiscoveries == true) {
                var allDiscoveries = Model.Discoveries.GetAllOrdered();
 
                var report =
                    "=====================\r\n" +
                    "Functions          \r\n" +
                    "=====================\r\n" +
                    Model.Discoveries.CreateReport(allDiscoveries.Where(x => x.Type == DiscoveredDataType.Function).ToArray(), false) + "\r\n" +
                    "=====================\r\n" +
                    "Arrays             \r\n" +
                    "=====================\r\n" +
                    Model.Discoveries.CreateReport(allDiscoveries.Where(x => x.Type == DiscoveredDataType.Array).ToArray(), false) + "\r\n" +
                    "=====================\r\n" +
                    "Identified Pointers\r\n" +
                    "=====================\r\n" +
                    Model.Discoveries.CreateReport(allDiscoveries.Where(x => x.Type == DiscoveredDataType.Pointer && !x.IsUnidentifiedPointer).ToArray(), false) + "\r\n" +
                    "=====================\r\n" +
                    "Unidentified Pointers\r\n" +
                    "=====================\r\n" +
                    Model.Discoveries.CreateReport(allDiscoveries.Where(x => x.Type == DiscoveredDataType.Pointer && x.IsUnidentifiedPointer).ToArray(), false) + "\r\n" +
                    "=====================\r\n" +
                    "Unknowns             \r\n" +
                    "=====================\r\n" +
                    Model.Discoveries.CreateReport(allDiscoveries.Where(x => x.Type == DiscoveredDataType.Unknown).ToArray(), false) + "\r\n" +
                    "=====================\r\n" +
                    "Everything         \r\n" +
                    "=====================\r\n" +
                    Model.Discoveries.CreateReport(allDiscoveries, true);

                CreateChild(new TextView("Discovered Data", report));
            }

            return Control;
        }

        public IX1_File Model { get; }
    }
}
