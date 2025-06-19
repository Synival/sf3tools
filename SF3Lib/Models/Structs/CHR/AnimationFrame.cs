using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.CHR {
    public class AnimationFrame : Struct {
        private readonly int _frameIdAddr;
        private readonly int _durationAddr;

        public AnimationFrame(IByteData data, int id, string name, int address, int spriteIndex, int spriteId, int directions, int animIndex)
        : base(data, id, name, address, 0x4) {
            SpriteIndex = spriteIndex;
            SpriteID    = spriteId;
            Directions  = directions;
            AnimIndex   = animIndex;

            _frameIdAddr  = Address + 0x00; // 2 bytes
            _durationAddr = Address + 0x02; // 2 bytes
        }

        [TableViewModelColumn(addressField: null, displayOrder: -0.4f, displayFormat: "X2")]
        public int SpriteIndex { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.3f, displayFormat: "X2", minWidth: 200)]
        [NameGetter(NamedValueType.Sprite)]
        public int SpriteID { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.2f)]
        public int Directions { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.1f, displayFormat: "X2")]
        public int AnimIndex { get; }

        [TableViewModelColumn(addressField: nameof(_frameIdAddr), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public int FrameID {
            get => Data.GetWord(_frameIdAddr);
            set => Data.SetWord(_frameIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_durationAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int Duration {
            get => Data.GetWord(_durationAddr);
            set => Data.SetWord(_durationAddr, value);
        }
    }
}
