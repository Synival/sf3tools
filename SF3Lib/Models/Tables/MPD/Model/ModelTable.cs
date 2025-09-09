using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Tables.MPD.Model {
    public class ModelTable : FixedSizeTable<Structs.MPD.Model.Model> {
        protected ModelTable(IByteData data, string name, int address, int count, bool hasTagsAndFlags, ModelCollectionType collectionType)
        : base(data, name, address, count) {
            HasTagsAndFlags = hasTagsAndFlags;
            CollectionType = collectionType;
        }

        public static ModelTable Create(IByteData data, string name, int address, int count, bool hasTagsAndFlags, ModelCollectionType collectionType)
            => Create(() => new ModelTable(data, name, address, count, hasTagsAndFlags, collectionType));

        public override bool Load()
            => Load((id, address) => new Structs.MPD.Model.Model(Data, id, "Model" + id.ToString("D4"), address, HasTagsAndFlags, CollectionType));

        public bool HasTagsAndFlags { get; }
        public ModelCollectionType CollectionType { get; }
    }
}
