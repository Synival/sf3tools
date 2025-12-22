using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Animation;
using SF3.Models.Tables.MPD.Animation;

namespace SF3.Models.Structs.MPD {
    public class TextureAnimation : Struct {
        private readonly int _textureIdAddr;
        private readonly int _widthAddr;
        private readonly int _heightAddr;
        private readonly int _frameTimerStartAddr;

        public TextureAnimation(IByteData data, int id, string name, int address, bool is32Bit, IMPD_File mpdFile)
        : base(data, id, name, address, 0x0A) {
            Is32Bit  = is32Bit;
            MPD_File = mpdFile;

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

            var frames = new List<TextureAnimationFrame>();

            // This happens in Scn2 SARA23.MPD for some reason...
            if (TextureIDRaw == _frameEndOffset)
                pos = Address + _bytesPerProperty;
            else if (TextureIDRaw != _textureEndId) {
                TextureAnimationFrameTable = TextureAnimationFrameTable.Create(data, "TexAnimFrames_" + id, pos, is32Bit, MPD_File, this);
                pos += TextureAnimationFrameTable.SizeInBytesPlusTerminator;
            }

            Size = pos - Address;
        }

        public bool Is32Bit { get; }
        public IMPD_File MPD_File { get; }
        public int FramesAddress { get; }

        public uint TextureIDRaw {
            get => Data.GetData(_textureIdAddr, _bytesPerProperty);
            set => Data.SetData(_textureIdAddr, value, _bytesPerProperty);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_textureIdAddr), displayName: "Texture ID", displayOrder: 0, displayFormat: "X2")]
        public uint TextureID {
            get => TextureIDRaw & 0xFF;
            set => TextureIDRaw = (TextureIDRaw & ~0xFFu) | (value & 0xFF);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_textureIdAddr), displayOrder: 0.1f)]
        public bool IsIndexed {
            get => (TextureIDRaw & 0x100) == 0x100;
            set => TextureIDRaw = (TextureIDRaw & ~0x100u) | (value ? 0x100u : 0);
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
        public int NumFrames => TextureAnimationFrameTable?.Length ?? 0;

        [BulkCopyRecurse]
        public TextureAnimationFrameTable TextureAnimationFrameTable { get; } = null;

        private readonly int _bytesPerProperty;
        private readonly uint _textureEndId;
        private readonly uint _frameEndOffset;
    }
}
