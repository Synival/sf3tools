using System;
using SF3.ByteData;
using SF3.Models.Structs.IconPointer;

namespace SF3.Models.Tables.IconPointer {
    public class SpellIconTable : ResourceTable<SpellIcon> {
        protected SpellIconTable(IByteData data, string name, string resourceFile, int address, bool has16BitIconAddr, int realOffsetStart)
        : base(data, name, resourceFile, address, 256) {
            Has16BitIconAddr = has16BitIconAddr;
            RealOffsetStart  = realOffsetStart;
        }

        public static SpellIconTable Create(IByteData data, string name, string resourceFile, int address, bool has16BitIconAddr, int realOffsetStart) {
            var newTable = new SpellIconTable(data, name, resourceFile, address, has16BitIconAddr, realOffsetStart);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new SpellIcon(Data, id, name, address, Has16BitIconAddr, RealOffsetStart));

        public bool Has16BitIconAddr { get; }
        public int RealOffsetStart { get; }
    }
}
