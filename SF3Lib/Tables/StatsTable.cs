using System;
using SF3.FileEditors;
using SF3.Models;
using SF3.Types;
using static SF3.Utils.Resources;

namespace SF3.Tables {
    public class StatsTable : Table<Stats> {
        public StatsTable(ISF3FileEditor fileEditor) : base(fileEditor) {
            ResourceFile = ResourceFileForScenario(Scenario, "ClassList.xml");

            var checkType     = FileEditor.GetByte(0x00000009);  // if it's 0x07 we're in a x033.bin
            var checkVersion2 = FileEditor.GetByte(0x000000017); // to determine which version of scn2 we are using 
            var isX033       = checkType == 0x07;

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    Address = isX033 ? 0x00000da4 : 0x00000d74;
                    break;

                case ScenarioType.Scenario2: {
                    if (isX033) {
                        var isScn2Ver1003 = checkVersion2 == 0x8c;
                        Address = isScn2Ver1003 ? 0x00000ee0 : 0x00000f08;
                    }
                    else {
                        var isScn2Ver1003 = checkVersion2 == 0x4c;
                        Address = isScn2Ver1003 ? 0x00000ea4 : 0x00000eb4;
                    }
                    break;
                }

                case ScenarioType.Scenario3:
                    Address = isX033 ? 0x00001030 : 0x00000ff4;
                    break;

                case ScenarioType.PremiumDisk:
                    Address = isX033 ? 0x00001204 : 0x000011bc;
                    break;

                default:
                    throw new ArgumentException(nameof(Scenario));
            }
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Stats(FileEditor, id, name, address, Scenario));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 300;
    }
}
