using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Structs.MPD.TextureAnimation;
using SF3.Models.Tables.MPD.TextureAnimation;

namespace SF3.Models.Structs.MPD {
    public class TextureAnimationModel : Struct {
        private readonly int _textureIdAddr;
        private readonly int _widthAddr;
        private readonly int _heightAddr;
        private readonly int _frameTimerStartAddr;

        public TextureAnimationModel(IByteData data, int id, string name, int address, bool is32Bit)
        : base(data, id, name, address, 0x0A) {
            Is32Bit = is32Bit;

            _bytesPerProperty    = Is32Bit ? 0x04 : 0x02;
            _textureEndId        = Is32Bit ? 0xFFFF_FFFF : 0xFFFF;
            _frameEndOffset      = Is32Bit ? 0xFFFF_FFFE : 0xFFFE;

            _textureIdAddr       = Address + 0x00 * _bytesPerProperty;
            _widthAddr           = Address + 0x01 * _bytesPerProperty;
            _heightAddr          = Address + 0x02 * _bytesPerProperty;
            _frameTimerStartAddr = Address + 0x03 * _bytesPerProperty;
            FramesAddress        = Address + 0x04 * _bytesPerProperty; // variable sizes

            // Determine the number of frames. That will determine the size of this animation.
            var pos = FramesAddress;

            var frames = new List<FrameModel>();

            // This happens in Scn2 SARA23.MPD for some reason...
            if (TextureID == _frameEndOffset)
                pos = Address + _bytesPerProperty;
            else if (TextureID != _textureEndId) {
                FrameTable = FrameTable.Create(data, "TexAnimFrames_" + id, pos, is32Bit, (int) TextureID, (int) Width, (int) Height, id);
                pos += FrameTable.SizeInBytesPlusTerminator;
            }

            Size = pos - Address;
        }

        public bool Is32Bit { get; }
        public int FramesAddress { get; }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_textureIdAddr), displayName: "Texture ID", displayOrder: 0, displayFormat: "X2")]
        public uint TextureID {
            get => Data.GetData(_textureIdAddr, _bytesPerProperty);
            set => Data.SetData(_textureIdAddr, value, _bytesPerProperty);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_widthAddr), displayName: "Width", displayOrder: 1)]
        public uint Width {
            get => Data.GetData(_widthAddr, _bytesPerProperty);
            set => Data.SetData(_widthAddr, value, _bytesPerProperty);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_heightAddr), displayName: "Height", displayOrder: 2)]
        public uint Height {
            get => Data.GetData(_heightAddr, _bytesPerProperty);
            set => Data.SetData(_heightAddr, value, _bytesPerProperty);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_frameTimerStartAddr), displayName: "Frame Timer Start", displayOrder: 3)]
        public int FrameTimerStart {
            get => (int) Data.GetData(_frameTimerStartAddr, _bytesPerProperty);
            set => Data.SetData(_frameTimerStartAddr, (uint) value, _bytesPerProperty);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: null, displayName: "# Frames", displayOrder: 4, isReadOnly: true)]
        public int NumFrames => FrameTable?.Length ?? 0;

        [BulkCopyRecurse]
        public FrameTable FrameTable { get; } = null;

        private readonly int _bytesPerProperty;
        private readonly uint _textureEndId;
        private readonly uint _frameEndOffset;
    }
}
