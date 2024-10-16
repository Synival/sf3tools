using SF3.Models;
using SF3.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.X019_Editor
{
    public class X019_FileEditor : SF3FileEditor, IX019_FileEditor
    {
        public X019_FileEditor(ScenarioType scenario) : base(scenario)
        {
        }

        public override IEnumerable<IModelArray> MakeModelArrays()
        {
            return new List<IModelArray>();
        }
    }
}
