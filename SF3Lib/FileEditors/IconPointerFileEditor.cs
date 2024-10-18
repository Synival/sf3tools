using SF3.Attributes;
using SF3.FileEditors;
using SF3.Models.IconPointerEditor.ItemIcons;
using SF3.Models.IconPointerEditor.SpellIcons;
using SF3.Models;
using SF3.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.FileEditors
{
    public class IconPointerFileEditor : SF3FileEditor, IIconPointerFileEditor
    {
        public IconPointerFileEditor(ScenarioType scenario, bool isX026) : base(scenario)
        {
            IsX026 = isX026;
        }

        public override IEnumerable<IModelArray> MakeModelArrays()
        {
            return new List<IModelArray>()
            {
                (SpellIconList = new SpellIconList(this)),
                (ItemIconList = new ItemIconList(this))
            };
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
