using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class ModelsHeaderTable : FixedSizeTable<ModelsHeader> {
        protected ModelsHeaderTable(IByteData data, int address) : base(data, address, 1) {}

        public static ModelsHeaderTable Create(IByteData data, int address) {
            var newTable = new ModelsHeaderTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ModelsHeader(Data, id, "Header", address));
    }
}
