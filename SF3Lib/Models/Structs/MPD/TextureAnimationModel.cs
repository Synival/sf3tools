using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.MPD {
    public class TextureAnimationModel : Struct {
        private readonly int _textureIdAddress;
        private readonly int _widthAddress;
        private readonly int _heightAddress;
        private readonly int _unknownAddress;

        public TextureAnimationModel(IRawData data, int id, string name, int address, bool is32Bit)
        : base(data, id, name, address, 0x0A) {
            Is32Bit = is32Bit;

            _bytesPerProperty = Is32Bit ? 0x04 : 0x02;
            _textureEndId     = Is32Bit ? 0xFFFF_FFFF : 0xFFFF;
            _frameEndOffset   = Is32Bit ? 0xFFFF_FFFE : 0xFFFE;

            _textureIdAddress = Address + 0x00 * _bytesPerProperty;
            _widthAddress     = Address + 0x01 * _bytesPerProperty;
            _heightAddress    = Address + 0x02 * _bytesPerProperty;
            _unknownAddress   = Address + 0x03 * _bytesPerProperty;
            FramesAddress     = Address + 0x04 * _bytesPerProperty; // variable sizes

            // Determine the number of frames. That will determine the size of this animation.
            // TODO: how in the world do we model this?? a separate table each?
            var frameCount = 0;
            var pos = FramesAddress;

            if (TextureID != _textureEndId) {
                while (true) {
                    var frameOffset = Data.GetData(pos, _bytesPerProperty);
                    pos += _bytesPerProperty;
                    if (frameOffset == _frameEndOffset)
                        break;
                    pos += _bytesPerProperty;
                    frameCount++;
                }
            }

            Size = pos - Address;
            NumFrames = frameCount;
        }

        public bool Is32Bit { get; }
        public int FramesAddress { get; }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Texture ID", displayOrder: 0, displayFormat: "X2")]
        public uint TextureID {
            get => Data.GetData(_textureIdAddress, _bytesPerProperty);
            set => Data.SetData(_textureIdAddress, value, _bytesPerProperty);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Width", displayOrder: 1)]
        public uint Width {
            get => Data.GetData(_widthAddress, _bytesPerProperty);
            set => Data.SetData(_widthAddress, value, _bytesPerProperty);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Height", displayOrder: 2)]
        public uint Height {
            get => Data.GetData(_heightAddress, _bytesPerProperty);
            set => Data.SetData(_heightAddress, value, _bytesPerProperty);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Unknown", displayOrder: 3, displayFormat: "X4")]
        public uint Unknown {
            get => Data.GetData(_unknownAddress, _bytesPerProperty);
            set => Data.SetData(_unknownAddress, value, _bytesPerProperty);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "# Frames", displayOrder: 4, isReadOnly: true)]
        public int NumFrames { get; }

        private readonly int _bytesPerProperty;
        private readonly uint _textureEndId;
        private readonly uint _frameEndOffset;
    }
}
