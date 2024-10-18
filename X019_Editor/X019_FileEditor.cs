using SF3.Models;
using SF3.Types;
using SF3.X019_Editor.Models.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.X019_Editor
{
    public class X019_FileEditor : SF3FileEditor, IX019_FileEditor
    {
        public X019_FileEditor(ScenarioType scenario, bool isX044) : base(scenario)
        {
            IsX044 = isX044;
        }

        public override IEnumerable<IModelArray> MakeModelArrays()
        {
            return new List<IModelArray>()
            {
                (MonsterList = new MonsterList(this, IsX044))
            };
        }

        public MonsterList MonsterList { get; private set; }

        public bool IsX044 { get; }

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + ((Scenario == ScenarioType.PremiumDisk && IsX044) ? " (PD X044)" : "")
            : base.BaseTitle;
    }
}
