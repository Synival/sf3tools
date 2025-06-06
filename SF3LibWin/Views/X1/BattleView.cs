﻿using System.Windows.Forms;
using SF3.Models.Files.X1;

namespace SF3.Win.Views.X1 {
    public class BattleView : TabView {
        public BattleView(string name, Battle model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            base.Create();

            var ngc = Model.NameGetterContext;
            if (Model.BattleHeader != null)
                CreateChild(new DataModelView("Header", Model.BattleHeader, ngc));
            if (Model.SlotTable != null) {
                CreateChild(new TableView("Slots 1",     Model.SlotTable, ngc, displayGroups: ["Metadata", "Page1"]));
                CreateChild(new TableView("Slots 2",     Model.SlotTable, ngc, displayGroups: ["Metadata", "Page2"]));
                CreateChild(new TableView("Slots 3 (Conditions)", Model.SlotTable, ngc, displayGroups: ["Metadata", "Page3"]));
                CreateChild(new TableView("Slots 4 (AI)",    Model.SlotTable, ngc, displayGroups: ["Metadata", "Page4"]));
                CreateChild(new TableView("Slots 5 (Flags)", Model.SlotTable, ngc, displayGroups: ["Metadata", "Page5"]));
            }
            if (Model.SpawnZoneTable != null)
                CreateChild(new TableView("Spawn Zones", Model.SpawnZoneTable, ngc));
            if (Model.AITargetPositionTable != null)
                CreateChild(new TableView("AI Target Position", Model.AITargetPositionTable, ngc));
            if (Model.ScriptedMovementTable != null)
                CreateChild(new TableView("Scripted Movement", Model.ScriptedMovementTable, ngc));

            return Control;
        }

        public Battle Model { get; }
    }
}
