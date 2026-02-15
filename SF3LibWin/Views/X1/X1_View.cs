using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.X1;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.X1;
using SF3.Models.Tables.X1.Town;

namespace SF3.Win.Views.X1 {
    public class X1_View : TabView {
        public X1_View(string name, IX1_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.InteractableTables?.Any() == true) {
                var count = Model.InteractableTables.Count();
                var countStr = (count > 1) ? $" ({count})" : "";
                CreateChild(new TableArrayView<InteractableTable>("Interactables" + countStr, Model.InteractableTables.ToArray(), ngc));
            }

            if (Model.BattlePointersTable != null)
                CreateChild(new TableView("Battle Pointers", Model.BattlePointersTable, ngc));

            if (Model.NpcTables?.Any() == true) {
                var count = Model.NpcTables.Count();
                var countStr = (count > 1) ? $" ({count})" : "";
                CreateChild(new TableArrayView<NpcTable>("NPCs" + countStr, Model.NpcTables.ToArray(), ngc));
            }

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

            if (Model.ModelInstanceGroupTablesByAddress?.Count > 0) {
                var count = Model.ModelInstanceGroupTablesByAddress.Count;
                var countStr = (count > 1) ? $" ({count})" : "";
                CreateChild(new TableArrayView<ModelInstanceGroupTable>("Model Instance Groups" + countStr, Model.ModelInstanceGroupTablesByAddress.Values.ToArray(), ngc));
            }

            if (Model.ModelInstanceTablesByAddress?.Count > 0) {
                var count = Model.ModelInstanceTablesByAddress.Count;
                var countStr = (count > 1) ? $" ({count})" : "";
                CreateChild(new TableArrayView<ModelInstanceTable>("Model Instances" + countStr, Model.ModelInstanceTablesByAddress.Values.ToArray(), ngc));
            }

            if (Model.MapUpdateFuncTable != null)
                CreateChild(new TableView("Map Update Functions", Model.MapUpdateFuncTable, ngc));

            if (Model.BlacksmithTables?.Any() == true) {
                var count = Model.BlacksmithTables.Count();
                var countStr = (count > 1) ? $" ({count})" : "";
                CreateChild(new TableArrayView<BlacksmithTable>("Blacksmith" + countStr, Model.BlacksmithTables.ToArray(), ngc));
            }

            if (Model.BattleTalkTable != null)
                CreateChild(new TableView("Battle Talk", Model.BattleTalkTable, ngc));

            if (Model.ScriptsByAddress?.Count > 0) {
                var count = Model.ScriptsByAddress.Count;
                var countStr = (count > 1) ? $" ({count})" : "";
                CreateChild(new TextArrayView("Scripts" + countStr,
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

            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX1_File Model { get; }
    }
}
