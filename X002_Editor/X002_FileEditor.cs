using SF3.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.X002_Editor
{
    public class X002_FileEditor : SF3FileEditor, IX002_FileEditor
    {
        public X002_FileEditor(ScenarioType scenario) : base(scenario)
        {
        }
    }
}
