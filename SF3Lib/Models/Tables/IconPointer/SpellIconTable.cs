using CommonLib.NamedValues;
using SF3.Models.Structs.IconPointer;
using SF3.Models.Tables;
using SF3.RawData;
using SF3.Types;

namespace SF3.Models.Tables.IconPointer {
    public class SpellIconTable : Table<SpellIcon> {
        public SpellIconTable(IByteData data, string resourceFile, int address, bool has16BitIconAddr, int realOffsetStart)
        : base(data, resourceFile, address) {
            Has16BitIconAddr = has16BitIconAddr;
            RealOffsetStart  = realOffsetStart;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpellIcon(Data, id, name, address, Has16BitIconAddr, RealOffsetStart));

        public override int? MaxSize => 256;

        public bool Has16BitIconAddr { get; }
        public int RealOffsetStart { get; }
    }
}
