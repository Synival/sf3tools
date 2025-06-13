using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Files.X031 {
    public class X031_File : ScenarioTableFile, IX031_File {
        public override int RamAddress { get; }

        protected X031_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
            RamAddress = (Scenario >= ScenarioType.Scenario2) ? 0x0604C800 : 0x06053000;
        }

        public static X031_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X031_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var checkVersion2 = Data.GetByte(0x00000017); //if it's 0x7c we're in a x033.bin version 1.003 scn2
            var isScn2Ver1003  = checkVersion2 == 0x4c;

            int statsAddress;
            int initialInfoAddress;
            int weaponLevelAddress;

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    weaponLevelAddress = 0x0d64;
                    statsAddress       = 0x0d74;
                    initialInfoAddress = 0x1d50;
                    break;

                case ScenarioType.Scenario2:
                    weaponLevelAddress = isScn2Ver1003 ? 0x0e94 : 0x0ea4;
                    statsAddress       = isScn2Ver1003 ? 0x0ea4 : 0x0eb4;
                    initialInfoAddress = isScn2Ver1003 ? 0x2e5a : 0x2e6a;
                    break;

                case ScenarioType.Scenario3:
                    weaponLevelAddress = 0x0fe4;
                    statsAddress       = 0x0ff4;
                    initialInfoAddress = 0x54aa;
                    break;

                case ScenarioType.PremiumDisk:
                    weaponLevelAddress = 0x11ac;
                    statsAddress       = 0x11bc;
                    initialInfoAddress = 0x56ec;
                    break;

                default:
                    throw new ArgumentException(nameof(Scenario));
            }

            WeaponLevelExp = new WeaponLevel(Data, 0, "WeaponLevelExp", weaponLevelAddress);

            return new List<ITable>() {
                (StatsTable       = StatsTable.Create      (Data, "Stats",        ResourceFileForScenario(Scenario, "ClassList.xml"), statsAddress)),
                (InitialInfoTable = InitialInfoTable.Create(Data, "InitialStats", ResourceFileForScenario(Scenario, "ClassEquip.xml"), initialInfoAddress)),
            };
        }

        [BulkCopyRecurse]
        public WeaponLevel WeaponLevelExp { get; private set; }

        [BulkCopyRecurse]
        public StatsTable StatsTable { get; private set; }

        [BulkCopyRecurse]
        public InitialInfoTable InitialInfoTable { get; private set; }
    }
}
