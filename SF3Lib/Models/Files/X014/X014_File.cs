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
        public readonly int c_ramOffset = 0x06088000;

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

            if (MPDBattleSceneInfoTable != null) {
                var addresses = MPDBattleSceneInfoTable
                    .Select(x => x.BattleSceneFileID)
                    .Where(x => x > 0x0600_0000)
                    .Distinct()
                    .ToList();

                // The Premium Disk has this, but it's unreferenced. Add if it doesn't exist.
                if (Scenario == ScenarioType.PremiumDisk && !addresses.Contains(0x120D0 + c_ramOffset))
                    addresses.Add(0x120D0 + c_ramOffset);

                TerrainBasedBattleSceneTablesByRamAddress = addresses
                    .ToDictionary(x => x, x => TerrainBasedBattleSceneTable.Create(Data, "TerrainBasedBattleSceneTable @" + x.ToString("X"), (x - c_ramOffset)));
                tables.AddRange(TerrainBasedBattleSceneTablesByRamAddress.Values);
            }

            return tables;
        }

        public MPDBattleSceneInfoTable MPDBattleSceneInfoTable { get; private set; } = null;
        public Dictionary<int, TerrainBasedBattleSceneTable> TerrainBasedBattleSceneTablesByRamAddress { get; private set; } = null;
    }
}
