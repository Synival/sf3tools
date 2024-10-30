using SF3.FileEditors;
using SF3.Models;
using SF3.NamedValues;
using static SF3.Utils.Resources;

namespace SF3.Tables {
    public class SpellIconTable : Table<SpellIcon> {
        public SpellIconTable(ISF3FileEditor fileEditor, int address, bool has16BitIconAddr, int realOffsetStart) : base(fileEditor) {
            ResourceFile     = ResourceFileForScenario(Scenario, "SpellIcons.xml");
            Address          = address;
            Has16BitIconAddr = has16BitIconAddr;
            RealOffsetStart  = realOffsetStart;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpellIcon(
                FileEditor, id, name, address, "" /*ValueNames.SpellInfo.Info[Scenario].Values[id]*/, Has16BitIconAddr, RealOffsetStart
            ));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 256;

        public bool Has16BitIconAddr { get; }
        public int RealOffsetStart { get; }
    }
}
