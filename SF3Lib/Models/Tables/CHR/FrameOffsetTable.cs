using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class FrameOffsetTable : TerminatedTable<FrameOffset> {
        protected FrameOffsetTable(IByteData data, string name, int address) : base(data, name, address, 4, 100) {}

        public static FrameOffsetTable Create(IByteData data, string name, int address) {
            var newTable = new FrameOffsetTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load((id, address) => new FrameOffset(Data, id, $"{nameof(FrameOffset)}{id:D2}", address),
                (rows, prevRow) => prevRow.Offset != 0x00,
                false);
        }
    }
}
