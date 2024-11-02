using CommonLib.NamedValues;
using SF3.FileEditors;
using SF3.Models;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables {
    public class SpellIconTable : Table<SpellIcon> {
        public SpellIconTable(ISF3FileEditor fileEditor, int address, INameGetterContext nameContext, bool has16BitIconAddr, int realOffsetStart) : base(fileEditor) {
            ResourceFile     = ResourceFileForScenario(Scenario, "SpellIcons.xml");
            Address          = address;
            NameContext      = nameContext;
            Has16BitIconAddr = has16BitIconAddr;
            RealOffsetStart  = realOffsetStart;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpellIcon(
                FileEditor, id, name, address, NameContext.GetName(null, null, id, NamedValueType.Spell), Has16BitIconAddr, RealOffsetStart
            ));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 256;

        public INameGetterContext NameContext { get; }
        public bool Has16BitIconAddr { get; }
        public int RealOffsetStart { get; }
    }
}
