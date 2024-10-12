using SF3.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.X013_Editor
{
    public class X013_FileEditor : SF3FileEditor, IX013_FileEditor
    {
        public X013_FileEditor(ScenarioType scenario) : base(scenario)
        {
        }
    }
}
