using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.Tables;
using SF3.Tables.IconPointerEditor.ItemIcon;
using SF3.Tables.IconPointerEditor.SpellIcon;
using SF3.Types;

namespace SF3.FileEditors {
    public class IconPointerFileEditor : SF3FileEditor, IIconPointerFileEditor {
        public IconPointerFileEditor(ScenarioType scenario, bool isX026) : base(scenario) {
            IsX026 = isX026;
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (SpellIconList = new SpellIconList(this)),
                (ItemIconList = new ItemIconList(this))
            };
        }

        public override void DestroyTables() {
            SpellIconList = null;
            ItemIconList = null;
        }

        public bool IsX026 { get; }

        [BulkCopyRecurse]
        public SpellIconList SpellIconList { get; private set; }

        [BulkCopyRecurse]
        public ItemIconList ItemIconList { get; private set; }

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + (IsX026 ? " (X026)" : "")
            : base.BaseTitle;
    }
}
