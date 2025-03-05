using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class MovableModelTable : TerminatedTable<MovableModel> {
        protected MovableModelTable(IByteData data, string name, int address) : base(data, name, address) {
        }

        public static MovableModelTable Create(IByteData data, string name, int address) {
            var newTable = new MovableModelTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, address) => new MovableModel(Data, id, "MovableModel" + id.ToString("D2"), address),
                (rows, lastRow) => lastRow.PDataOffset != 0x00,
                false);
        }
    }
}
