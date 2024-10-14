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
        public X1_FileEditor(ScenarioType scenario, MapType map) : base(scenario)
        {
            Map = map;
        }

        private MapType _map;

        public MapType Map
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

        public int MapOffset => (int) Map;

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + " (Map: " + Map.ToString() + ")"
            : base.BaseTitle;
    }
}
