using System;
using SF3.FileEditors;
using SF3.Models;
using SF3.Types;

namespace SF3.Tables {
    public class WeaponLevelTable : Table<WeaponLevel> {

        public WeaponLevelTable(IX033_X031_FileEditor fileEditor) : base(fileEditor) {
            var checkType     = FileEditor.GetByte(0x00000009); //if it's 0x07 we're in a x033.bin
            var checkVersion2 = FileEditor.GetByte(0x00000017); //if it's 0x7c we're in a x033.bin version 1.003 scn2
            var isX033        = checkType == 0x07;

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    Address = isX033 ? 0x00000d94 : 0x00000d64;
                    break;

                case ScenarioType.Scenario2: {
                    if (isX033) {
                        var isSc2Ver1003 = checkVersion2 == 0x8c;
                        Address = isSc2Ver1003 ? 0x00000ed0 : 0x00000ef8;
                    }
                    else {
                        var isSc2Ver1003 = checkVersion2 == 0x4c;
                        Address = isSc2Ver1003 ? 0x00000e94 : 0x00000ea4;
                    }
                    break;
                }

                case ScenarioType.Scenario3:
                    Address = isX033 ? 0x00001020 : 0x00000fe4;
                    break;

                case ScenarioType.PremiumDisk:
                    Address = isX033 ? 0x000011f4 : 0x000011ac;
                    break;

                default:
                    throw new ArgumentException(nameof(Scenario));
            }
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new WeaponLevel(FileEditor, id, name, address));

        public override string ResourceFile => "Resources/WeaponLevel.xml";
        public override int Address { get; }
        public override int? MaxSize => 2;
    }
}
