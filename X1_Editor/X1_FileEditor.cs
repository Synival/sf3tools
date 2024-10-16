using SF3.Models;
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

        public override bool LoadFile(string filename)
        {
            if (!base.LoadFile(filename))
            {
                return false;
            }

            int offset = 0;
            int sub = 0;

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
            }

            else if (Scenario == ScenarioType.Other /* BTL99 */)
            {
                offset = 0x00000018; //btl99 initial pointer
                sub = 0x06060000;
            }

            offset = GetDouble(offset);

            offset = offset - sub; //first pointer
            offset = GetDouble(offset);

            /*A value higher means a pointer is on the offset, meaning we are in a battle. If it is not a 
              pointer we are at our destination so we know a town is loaded.
            */
            if (Scenario == ScenarioType.Scenario1 && offset > 0x0605F000)
            {
                IsBattle = true;
            }
            else if (offset > 0x0605e000)
            {
                IsBattle = true;
            }
            else
            {
                IsBattle = false;
            }

            return true;
        }

        public override IEnumerable<IModelArray> MakeModelArrays()
        {
            return new List<IModelArray>();
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

        private bool _isBattle;

        public bool IsBattle
        {
            get => _isBattle;
            set
            {
                if (_isBattle != value)
                {
                    _isBattle = value;
                    UpdateTitle();
                }
            }
        }

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + " (Map: " + Map.ToString() + ") (Type: " + (IsBattle ? "Battle" : "Town") + ")"
            : base.BaseTitle;
    }
}
