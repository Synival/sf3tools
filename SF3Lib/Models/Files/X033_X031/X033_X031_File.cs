using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.X033_X031;
using SF3.Models.Tables;
using SF3.Models.Tables.X033_X031;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Files.X033_X031 {
    public class X033_X031_File : ScenarioTableFile, IX033_X031_File {
        protected X033_X031_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static X033_X031_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X033_X031_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var checkType     = Data.GetByte(0x00000009);     //if it's 0x07 we're in a x033.bin
            var checkVersion2 = Data.GetByte(0x00000017); //if it's 0x7c we're in a x033.bin version 1.003 scn2
            var isX033        = checkType == 0x07;

            int statsAddress;
            int initialInfoAddress;
            int weaponLevelAddress;

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    weaponLevelAddress = isX033 ? 0x00000d94 : 0x00000d64;
                    statsAddress       = isX033 ? 0x00000da4 : 0x00000d74;
                    initialInfoAddress = isX033 ? 0x00001d80 : 0x00001d50;
                    break;

                case ScenarioType.Scenario2: {
                    if (isX033) {
                        var isScn2Ver1003  = checkVersion2 == 0x8c;
                        weaponLevelAddress = isScn2Ver1003 ? 0x00000ed0 : 0x00000ef8;
                        statsAddress       = isScn2Ver1003 ? 0x00000ee0 : 0x00000f08;
                        initialInfoAddress = isScn2Ver1003 ? 0x00002e96 : 0x00002ebe;
                    }
                    else {
                        var isScn2Ver1003  = checkVersion2 == 0x4c;
                        weaponLevelAddress = isScn2Ver1003 ? 0x00000e94 : 0x00000ea4;
                        statsAddress       = isScn2Ver1003 ? 0x00000ea4 : 0x00000eb4;
                        initialInfoAddress = isScn2Ver1003 ? 0x00002e5a : 0x00002e6a;
                    }
                    break;
                }

                case ScenarioType.Scenario3:
                    weaponLevelAddress = isX033 ? 0x00001020 : 0x00000fe4;
                    statsAddress       = isX033 ? 0x00001030 : 0x00000ff4;
                    initialInfoAddress = isX033 ? 0x000054e6 : 0x000054aa;
                    break;

                case ScenarioType.PremiumDisk:
                    weaponLevelAddress = isX033 ? 0x000011f4 : 0x000011ac;
                    statsAddress       = isX033 ? 0x00001204 : 0x000011bc;
                    initialInfoAddress = isX033 ? 0x00005734 : 0x000056ec;
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
