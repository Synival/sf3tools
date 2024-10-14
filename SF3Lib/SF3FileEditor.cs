using SF3.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3
{
    /// <summary>
    /// IFileEditor specifically for files in Shining Force 3
    /// </summary>
    public class SF3FileEditor : FileEditor, ISF3FileEditor
    {
        public SF3FileEditor(ScenarioType scenario)
        {
            Scenario = scenario;
        }

        public ScenarioType Scenario { get; }

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + " (" + Scenario.ToString() + ")"
            : base.BaseTitle;
    }
}
