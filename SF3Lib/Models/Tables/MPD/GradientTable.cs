using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class GradientTable : TerminatedTable<GradientModel> {
        protected GradientTable(IByteData data, string name, int address) : base(data, name, address, 2, null) { }

        public static GradientTable Create(IByteData data, string name, int address) {
            var newTable = new GradientTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, address) => new GradientModel(Data, id, "Gradient" + id.ToString("D2"), address),
                (rows, lastRow) => lastRow.StartPosition != 0xFFFF,
                addEndModel: false
            );
        }
    }
}
