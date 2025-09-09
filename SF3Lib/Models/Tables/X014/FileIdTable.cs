using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class AnimationLocationTable : ResourceTable<AnimationLocation> {
        protected AnimationLocationTable(IByteData data, string name, string resourceFile, int address, bool isEffectFileIndexes) : base(data, name, resourceFile, address, 0x100) {
            IsEffectFileIndexes = isEffectFileIndexes;
        }

        public static AnimationLocationTable Create(IByteData data, string name, string resourceFile, int address, bool isEffectFileIndexes)
            => Create(() => new AnimationLocationTable(data, name, resourceFile, address, isEffectFileIndexes));

        public override bool Load()
            => Load((id, name, address) => new AnimationLocation(Data, id, name, address, IsEffectFileIndexes));

        public bool IsEffectFileIndexes { get; }
    }
}
