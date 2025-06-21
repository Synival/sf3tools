using System.Linq;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Tables.CHR;
using SF3.Types;
using SF3.Utils;

namespace SF3.Models.Structs.CHR {
    public class AnimationFrame : Struct {
        private readonly int _frameIdAddr;
        private readonly int _durationAddr;

        public AnimationFrame(IByteData data, int id, string name, int address, int spriteIndex, int spriteId, int directions, AnimationType animationType, FrameTable frameTable)
        : base(data, id, name, address, 0x4) {
            SpriteIndex = spriteIndex;
            SpriteID    = spriteId;
            Directions  = directions;
            AnimationType = animationType;
            FrameTable  = frameTable;

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

        [TableViewModelColumn(addressField: null, displayOrder: -0.1f)]
        public AnimationType AnimationType { get; }

        public FrameTable FrameTable { get; }

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

        [TableViewModelColumn(displayOrder: 2)]
        public bool IsEndingFrame => FrameID >= 0xF0 && FrameID != 0xF1;

        private bool _textureLoaded = false;
        private ITexture _texture = null;
        public ITexture Texture {
            get {
                LoadTextureIfNecessary();
                return _texture;
            }
        }

        private void LoadTextureIfNecessary() {
            if (_textureLoaded)
                return;
            _textureLoaded = true;

            if (FrameTable == null || _texture != null)
                return;

            var frameMin = FrameID;
            var frameMax = frameMin + Directions;
            var frames = FrameTable
                .Where(x => x.ID >= frameMin && x.ID < frameMax)
                .Select(x => x.Texture)
                .ToArray();

            _texture = TextureUtils.StackTextures(0, 0, 0, frames);
        }
    }
}
