using SF3.Models;
using SF3.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.X033_X031_Editor
{
    public class X033_X031_FileEditor : SF3FileEditor, IX033_X031_FileEditor
    {
        public X033_X031_FileEditor(ScenarioType scenario) : base(scenario)
        {
        }

        public override IEnumerable<IModelArray> MakeModelArrays()
        {
            return new List<IModelArray>();
        }
    }
}
