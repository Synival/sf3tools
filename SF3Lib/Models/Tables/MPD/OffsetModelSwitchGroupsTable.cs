using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class OffsetModelSwitchGroupsTable : TerminatedTable<Offset4Model> {
        protected OffsetModelSwitchGroupsTable(IByteData data, string name, int address) : base(data, name, address) {
        }

        public static OffsetModelSwitchGroupsTable Create(IByteData data, string name, int address) {
            var newTable = new OffsetModelSwitchGroupsTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, address) => new Offset4Model(Data, id, "Row " + id, address),
                (currentRows, model) => model.Unknown1 != 0xFFFF_FFFF,
                false);
        }
    }
}
