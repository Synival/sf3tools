using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
    public class AnimationLocation : Struct {
        private readonly int _locationAddr;

        public AnimationLocation(IByteData data, int id, string name, int address, bool isEffectFileIndexes) : base(data, id, name, address, isEffectFileIndexes ? 0x02 : 0x04) {
            IsEffectFileIndexes = isEffectFileIndexes;
            LocationType = IsEffectFileIndexes ? NamedValueType.EffectFileIndex : NamedValueType.FileIndex;

            _locationAddr = Address + 0x00; // 4 or 2 bytes
        }

        public bool IsEffectFileIndexes { get; }
        public NamedValueType LocationType { get; }

        [TableViewModelColumn(addressField: nameof(_locationAddr), displayOrder: 1, minWidth: 200, displayFormat: "X2")]
        [NameGetter(NamedValueType.ConditionalType, nameof(LocationType))]
        [BulkCopy]
        public int Location {
            get => IsEffectFileIndexes ? Data.GetWord(_locationAddr) : Data.GetDouble(_locationAddr);
            set {
                if (IsEffectFileIndexes)
                    Data.SetWord(_locationAddr, value);
                else
                    Data.SetDouble(_locationAddr, value);
            }
        }
    }
}
