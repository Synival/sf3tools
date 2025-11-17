using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Tables.MPD.Model {
    public class ModelInstanceTable : FixedSizeTable<Structs.MPD.Model.ModelInstance> {
        protected ModelInstanceTable(IByteData data, string name, int address, int count, bool hasTagsAndFlags, ModelCollectionType collectionType)
        : base(data, name, address, count) {
            HasTagsAndFlags = hasTagsAndFlags;
            CollectionType = collectionType;
        }

        public static ModelInstanceTable Create(IByteData data, string name, int address, int count, bool hasTagsAndFlags, ModelCollectionType collectionType)
            => Create(() => new ModelInstanceTable(data, name, address, count, hasTagsAndFlags, collectionType));

        public override bool Load()
            => Load((id, address) => new Structs.MPD.Model.ModelInstance(Data, id, "ModelInstance" + id.ToString("D4"), address, HasTagsAndFlags, CollectionType));

        public bool HasTagsAndFlags { get; }
        public ModelCollectionType CollectionType { get; }
    }
}
