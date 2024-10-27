using System;
using SF3.FileEditors;
using SF3.Models;
using SF3.Types;
using static SF3.Utils.Resources;

namespace SF3.Tables {
    public class ItemIconTable : Table<ItemIcon> {
        public ItemIconTable(ISF3FileEditor fileEditor, bool isX026) : base(fileEditor) {
            ResourceFile = ResourceFileForScenario(FileEditor.Scenario, "Items.xml");
            IsX026 = isX026;

            if (Scenario == ScenarioType.Scenario1) {
                int offset, sub;
                if (IsX026 == true) {
                    offset = 0x08f0; //scn1 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x0000003C; //scn1 initial pointer
                    sub = 0x06068000;
                }
                Address = FileEditor.GetDouble(offset) - sub;
            }
            else if (Scenario == ScenarioType.Scenario2) {
                int offset, sub;
                if (IsX026 == true) {
                    offset = 0x0a08; //scn2 x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x0000003C; //scn2 initial pointer
                    sub = 0x06068000;
                }
                Address = FileEditor.GetDouble(offset) - sub;
            }
            else if (Scenario == ScenarioType.Scenario3) {
                int offset, sub;
                if (IsX026 == true) {
                    offset = 0x09b4; //scn3 x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x0000003C; //scn3 initial pointer
                    sub = 0x06068000;
                }
                Address = FileEditor.GetDouble(offset) - sub;
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                int offset, sub;
                if (IsX026 == true) {
                    offset = 0x072c; //pd x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x0000003C; //pd initial pointer
                    sub = 0x06068000;
                }
                Address = FileEditor.GetDouble(offset) - sub;
            }
            else
                throw new ArgumentException(nameof(Scenario));
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new ItemIcon(FileEditor, id, name, address, IsSc1X026));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 300;

        public bool IsX026 { get; }
        public bool IsSc1X026 => FileEditor.Scenario == ScenarioType.Scenario1 && IsX026;
    }
}
