using SF3.Attributes;
using SF3.IconPointerEditor.Models.ItemIcons;
using SF3.IconPointerEditor.Models.SpellIcons;
using SF3.Models;
using SF3.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.IconPointerEditor
{
    public class IconPointerFileEditor : SF3FileEditor, IIconPointerFileEditor
    {
        public IconPointerFileEditor(ScenarioType scenario, bool x026) : base(scenario)
        {
            X026 = x026;
        }

        public override IEnumerable<IModelArray> MakeModelArrays()
        {
            return new List<IModelArray>()
            {
                (SpellIconList = new SpellIconList(this)),
                (ItemIconList = new ItemIconList(this))
            };
        }

        public bool X026 { get; }

        [BulkCopyRecurse]
        public SpellIconList SpellIconList { get; private set; }

        [BulkCopyRecurse]
        public ItemIconList ItemIconList { get; private set; }

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + (X026 ? " (X026)" : "")
            : base.BaseTitle;
    }
}
