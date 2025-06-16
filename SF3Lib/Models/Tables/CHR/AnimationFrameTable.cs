using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class AnimationFrameTable : TerminatedTable<AnimationFrame> {
        protected AnimationFrameTable(IByteData data, string name, int address, string rowPrefix)
        : base(data, name, address, 0x00, 100) {
            RowPrefix = rowPrefix ?? "";
        }

        public static AnimationFrameTable Create(IByteData data, string name, int address, string rowPrefix) {
            var newTable = new AnimationFrameTable(data, name, address, rowPrefix);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new AnimationFrame(Data, id, $"{RowPrefix}{nameof(AnimationFrame)}{id:D2}", addr),
                (rows, prevRow) => ((sbyte) prevRow.FrameID) >= 0,
                addEndModel: true // This last row has important data; we always want to include it
            );
        }

        public string RowPrefix { get; }
    }
}
