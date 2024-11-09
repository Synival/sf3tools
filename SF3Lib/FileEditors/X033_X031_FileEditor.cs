using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.NamedValues;
using SF3.Tables;
using SF3.Tables.X033_X031;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;
using static SF3.Utils.ResourceUtils;

namespace SF3.FileEditors {
    public class X033_X031_FileEditor : SF3FileEditor, IX033_X031_FileEditor {
        public X033_X031_FileEditor(ScenarioType scenario) : base(scenario, new NameGetterContext(scenario)) {
        }

        public override IEnumerable<ITable> MakeTables() {
            var checkType     = GetByte(0x00000009);     //if it's 0x07 we're in a x033.bin
            var checkVersion2 = GetByte(0x00000017); //if it's 0x7c we're in a x033.bin version 1.003 scn2
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

            return new List<ITable>() {
                (WeaponLevelTable = new WeaponLevelTable(this, ResourceFile("WeaponLevel.xml"), weaponLevelAddress)),
                (StatsTable       = new StatsTable(this, ResourceFileForScenario(Scenario, "ClassList.xml"), statsAddress)),
                (InitialInfoTable = new InitialInfoTable(this, ResourceFileForScenario(Scenario, "ClassEquip.xml"), initialInfoAddress)),
            };
        }

        public override void DestroyTables() {
            WeaponLevelTable = null;
            StatsTable       = null;
            InitialInfoTable = null;
        }

        [BulkCopyRecurse]
        public WeaponLevelTable WeaponLevelTable { get; private set; }

        [BulkCopyRecurse]
        public StatsTable StatsTable { get; private set; }

        [BulkCopyRecurse]
        public InitialInfoTable InitialInfoTable { get; private set; }
    }
}
