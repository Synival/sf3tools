﻿using SF3.Models;
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
        public X019_FileEditor(ScenarioType scenario, bool isPDX044) : base(scenario)
        {
            IsPDX044 = isPDX044;
        }

        public override IEnumerable<IModelArray> MakeModelArrays()
        {
            return new List<IModelArray>()
            {
                (MonsterList = new MonsterList(this, IsPDX044))
            };
        }

        public MonsterList MonsterList { get; private set; }

        public bool IsPDX044 { get; }
    }
}
