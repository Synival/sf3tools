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

namespace SF3.Models.Files.X033 {
    public class X033_File : ScenarioTableFile, IX033_File {
        public override int RamAddress => 0x06078000;
        public override int RamAddressLimit => 0x06080000;

        protected X033_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static X033_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X033_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var checkVersion2 = Data.GetByte(0x00000017); //if it's 0x7c we're in a x033.bin version 1.003 scn2
            var isScn2Ver1003 = checkVersion2 == 0x8c;

            int statsAddress;
            int initialInfoAddress;
            int weaponLevelAddress;

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    weaponLevelAddress = 0x0d94;
                    statsAddress       = 0x0da4;
                    initialInfoAddress = 0x1d80;
                    break;

                case ScenarioType.Scenario2:
                    weaponLevelAddress = isScn2Ver1003 ? 0x0ed0 : 0x0ef8;
                    statsAddress       = isScn2Ver1003 ? 0x0ee0 : 0x0f08;
                    initialInfoAddress = isScn2Ver1003 ? 0x2e96 : 0x2ebe;
                    break;

                case ScenarioType.Scenario3:
                    weaponLevelAddress = 0x1020;
                    statsAddress       = 0x1030;
                    initialInfoAddress = 0x54e6;
                    break;

                case ScenarioType.PremiumDisk:
                    weaponLevelAddress = 0x11f4;
                    statsAddress       = 0x1204;
                    initialInfoAddress = 0x5734;
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
