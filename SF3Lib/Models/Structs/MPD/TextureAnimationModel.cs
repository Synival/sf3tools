using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.Models.Structs.MPD.TextureAnimation;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    public class TextureAnimationModel : Struct {
        private readonly int _textureIdAddress;
        private readonly int _widthAddress;
        private readonly int _heightAddress;
        private readonly int _unknownAddress;

        public TextureAnimationModel(IByteData data, int id, string name, int address, bool is32Bit)
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
            var pos = FramesAddress;

            var frames = new List<FrameModel>();
            var frameId = 0;

            // This happens in Scn2 SARA23.MPD for some reason...
            if (TextureID == _frameEndOffset)
                pos = Address + _bytesPerProperty;
            else if (TextureID != _textureEndId) {
                while (true) {
                    var frameOffset = Data.GetData(pos, _bytesPerProperty);
                    if (frameOffset == _frameEndOffset) {
                        pos += _bytesPerProperty;
                        break;
                    }

                    var newFrame = new FrameModel(Data, frameId, "TexAnim" + id + "_" + (frameId + 1), pos, is32Bit, (int) TextureID, (int) Width, (int) Height, id, frameId + 1);
                    frames.Add(newFrame);
                    frameId++;
                    pos += newFrame.Size;
                }
            }

            Size = pos - Address;
            Frames = frames.ToArray();
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
        public int NumFrames => Frames?.Length ?? 0;

        [BulkCopyRecurse]
        public FrameModel[] Frames { get; } = null;

        private readonly int _bytesPerProperty;
        private readonly uint _textureEndId;
        private readonly uint _frameEndOffset;
    }
}
