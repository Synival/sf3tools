using CommonLib.NamedValues;
using SF3.Models.Structs.IconPointer;
using SF3.Models.Tables;
using SF3.ByteData;
using SF3.Types;
using System;

namespace SF3.Models.Tables.IconPointer {
    public class SpellIconTable : Table<SpellIcon> {
        protected SpellIconTable(IByteData data, string resourceFile, int address, bool has16BitIconAddr, int realOffsetStart)
        : base(data, resourceFile, address) {
            Has16BitIconAddr = has16BitIconAddr;
            RealOffsetStart  = realOffsetStart;
        }

        public static SpellIconTable Create(IByteData data, string resourceFile, int address, bool has16BitIconAddr, int realOffsetStart) {
            var newTable = new SpellIconTable(data, resourceFile, address, has16BitIconAddr, realOffsetStart);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpellIcon(Data, id, name, address, Has16BitIconAddr, RealOffsetStart));

        public override int? MaxSize => 256;

        public bool Has16BitIconAddr { get; }
        public int RealOffsetStart { get; }
    }
}
