using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class ModelsHeaderTable : Table<ModelsHeaderModel> {
        protected ModelsHeaderTable(IByteData data, int address) : base(data, address) {}

        public static ModelsHeaderTable Create(IByteData data, int address) {
            var newTable = new ModelsHeaderTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new ModelsHeaderModel(Data, id, "Header", address));

        public override int? MaxSize => 1;
    }
}
