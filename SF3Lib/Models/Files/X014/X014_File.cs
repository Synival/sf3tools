using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X014;
using SF3.Types;

namespace SF3.Models.Files.X014 {
    public class X014_File : ScenarioTableFile, IX014_File {
        public readonly int c_ramOffset = 0x06070000;

        protected X014_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static X014_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X014_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        private uint GetMPDBattleSceneInfoTableAddr() {
            switch (Scenario) {
            // TODO: Scenario 2 versions
            case ScenarioType.Scenario2:
                return 0x11A00;
            case ScenarioType.Scenario3:
                return 0x11EDC;
            case ScenarioType.PremiumDisk:
                return 0x12110;
            default:
                return 0;
            }
        }

        public override IEnumerable<ITable> MakeTables() {
            var mpdBattleSceneAddr = GetMPDBattleSceneInfoTableAddr();
            var tables = new List<ITable>() {
                (MPDBattleSceneInfoTable = (mpdBattleSceneAddr == 0) ? null : MPDBattleSceneInfoTable.Create(Data, "MPDBattleSceneInfoTable", (int) mpdBattleSceneAddr))
            }.Where(x => x != null).ToList();

            return tables;
        }

        public MPDBattleSceneInfoTable MPDBattleSceneInfoTable { get; private set; } = null;
    }
}
