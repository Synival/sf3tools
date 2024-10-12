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
    }
}
