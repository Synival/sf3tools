using System;
using SF3.FileEditors;
using SF3.Models;
using SF3.Types;
using static SF3.Utils.Resources;

namespace SF3.Tables {
    public class ItemIconTable : Table<ItemIcon> {
        public ItemIconTable(ISF3FileEditor fileEditor, int address, bool has16BitIconAddr) : base(fileEditor) {
            ResourceFile     = ResourceFileForScenario(Scenario, "Items.xml");
            Address          = address;
            Has16BitIconAddr = has16BitIconAddr;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new ItemIcon(FileEditor, id, name, address, IsSc1X026));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 300;

        public bool Has16BitIconAddr { get; }
        public bool IsSc1X026 => Scenario == ScenarioType.Scenario1 && Has16BitIconAddr;
    }
}
