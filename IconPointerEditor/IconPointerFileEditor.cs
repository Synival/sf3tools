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

        public bool X026 { get; }

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + (X026 ? " (X026)" : "")
            : base.BaseTitle;
    }
}
