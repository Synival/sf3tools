using SF3.FileEditors;
using SF3.Models;
using SF3.Types;
using static SF3.Utils.Resources;

namespace SF3.Tables {
    public class InitialInfoTable : Table<InitialInfo> {
        public InitialInfoTable(IX033_X031_FileEditor fileEditor) : base(fileEditor) {
            ResourceFile = ResourceFileForScenario(Scenario, "ClassEquip.xml");

            var checkType     = FileEditor.GetByte(0x00000009);     //if it's 0x07 we're in a x033.bin
            var checkVersion2 = FileEditor.GetByte(0x00000017); //if it's 0x7c we're in a x033.bin version 1.003 scn2

            var isX033 = checkType == 0x07;

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    Address = isX033 ? 0x00001d80 : 0x00001d50;
                    break;

                case ScenarioType.Scenario2: {
                    if (isX033) {
                        var isScn2Ver1003 = checkVersion2 == 0x8c;
                        Address = isScn2Ver1003 ? 0x00002e96 : 0x00002ebe;
                    }
                    else {
                        var isScn2Ver1003 = checkVersion2 == 0x4c;
                        Address = isScn2Ver1003 ? 0x00002e5a : 0x00002e6a;
                    }

                    break;
                }

                case ScenarioType.Scenario3:
                    Address = isX033 ? 0x000054e6 : 0x000054aa;
                    break;
                case ScenarioType.PremiumDisk:
                    Address = isX033 ? 0x00005734 : 0x000056ec;
                    break;
            }
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new InitialInfo(FileEditor, id, name, address, Scenario));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 100;
    }
}
