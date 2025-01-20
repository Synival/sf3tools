using System;
using SF3.ByteData;

namespace SF3.Models.Tables.MPD.Model {
    public class ModelTable : Table<Structs.MPD.Model.Model> {
        protected ModelTable(IByteData data, int address, int count) : base(data, address) {
            MaxSize = count;
        }

        public static ModelTable Create(IByteData data, int address, int count) {
            var newTable = new ModelTable(data, address, count);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new Structs.MPD.Model.Model(Data, id, "Model" + id.ToString("D4"), address));

        public override int? MaxSize { get; }
    }
}
