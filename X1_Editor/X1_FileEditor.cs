using SF3.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.X1_Editor
{
    public class X1_FileEditor : SF3FileEditor, IX1_FileEditor
    {
        public X1_FileEditor(ScenarioType scenario, int map) : base(scenario)
        {
            Map = map;
        }

        public int Map { get; set; }

        // TODO: just use an enum!
        public string MapString
        {
            get
            {
                switch (Map)
                {
                    case 0x00: return "Synbios";
                    case 0x04: return "Medion";
                    case 0x08: return "Julian";
                    case 0x0C: return "Extra";
                    default: return "(invalid value)";
                }
            }
        }

        public override string Title => IsLoaded
            ? base.Title + " (Map: " + MapString + ")"
            : base.Title;
    }
}
