using System.Collections.Generic;
using SF3.Attributes;
using SF3.Models;
using SF3.Models.X033_X031.InitialInfos;
using SF3.Models.X033_X031.Stats;
using SF3.Models.X033_X031.WeaponLevel;
using SF3.Types;

namespace SF3.FileEditors {
    public class X033_X031_FileEditor : SF3FileEditor, IX033_X031_FileEditor {
        public X033_X031_FileEditor(ScenarioType scenario) : base(scenario) {
        }

        public override IEnumerable<IModelArray> MakeModelArrays() {
            return new List<IModelArray>() {
                (StatsList = new StatsList(this)),
                (InitialInfoList = new InitialInfoList(this)),
                (WeaponLevelList = new WeaponLevelList(this)),
            };
        }

        [BulkCopyRecurse]
        public StatsList StatsList { get; private set; }

        [BulkCopyRecurse]
        public InitialInfoList InitialInfoList { get; private set; }

        [BulkCopyRecurse]
        public WeaponLevelList WeaponLevelList { get; private set; }
    }
}
