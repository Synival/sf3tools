using System;
using SF3.ByteData;

namespace SF3.Models.Tables.MPD.Model {
    public class ModelTable : FixedSizeTable<Structs.MPD.Model.Model> {
        protected ModelTable(IByteData data, string name, int address, int count, bool hasTagsAndFlags) : base(data, name, address, count) {
            HasTagsAndFlags = hasTagsAndFlags;
        }

        public static ModelTable Create(IByteData data, string name, int address, int count, bool hasTagsAndFlags) {
            var newTable = new ModelTable(data, name, address, count, hasTagsAndFlags);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new Structs.MPD.Model.Model(Data, id, "Model" + id.ToString("D4"), address, HasTagsAndFlags));

        public bool HasTagsAndFlags { get; }
    }
}
