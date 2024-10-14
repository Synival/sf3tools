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

        private int _map;

        public int Map
        {
            get => _map;
            set
            {
                if (_map != value)
                {
                    _map = value;
                    UpdateTitle();
                }
            }
        }

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

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + " (Map: " + MapString + ")"
            : base.BaseTitle;
    }
}
