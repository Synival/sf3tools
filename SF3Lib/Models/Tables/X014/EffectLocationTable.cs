using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class EffectLocationTable : ResourceTable<EffectLocation> {
        protected EffectLocationTable(IByteData data, string name, string resourceFile, int address, bool isEffectFileIndexes) : base(data, name, resourceFile, address, 0x100) {
            IsEffectFileIndexes = isEffectFileIndexes;
        }

        public static EffectLocationTable Create(IByteData data, string name, string resourceFile, int address, bool isEffectFileIndexes)
            => Create(() => new EffectLocationTable(data, name, resourceFile, address, isEffectFileIndexes));

        public override bool Load()
            => Load((id, name, address) => new EffectLocation(Data, id, name, address, IsEffectFileIndexes));

        public bool IsEffectFileIndexes { get; }
    }
}
