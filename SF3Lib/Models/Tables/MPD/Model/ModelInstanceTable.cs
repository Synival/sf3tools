using SF3.ByteData;
using SF3.MPD.Interfaces;

namespace SF3.Models.Tables.MPD.Model {
    public class ModelInstanceTable : FixedSizeTable<Structs.MPD.Model.ModelInstance> {
        protected ModelInstanceTable(IByteData data, IMPD_ModelCollection collection, string name, int address, int count, bool hasTagsAndFlags)
        : base(data, name, address, count) {
            Collection      = collection;
            HasTagsAndFlags = hasTagsAndFlags;
        }

        public static ModelInstanceTable Create(IByteData data, IMPD_ModelCollection collection, string name, int address, int count, bool hasTagsAndFlags)
            => Create(() => new ModelInstanceTable(data, collection, name, address, count, hasTagsAndFlags));

        public override bool Load()
            => Load((id, address) => new Structs.MPD.Model.ModelInstance(Data, Collection, id, "ModelInstance" + id.ToString("D4"), address, HasTagsAndFlags));

        public IMPD_ModelCollection Collection { get; }
        public bool HasTagsAndFlags { get; }
        public override int TerminatorSize => 4;
    }
}
