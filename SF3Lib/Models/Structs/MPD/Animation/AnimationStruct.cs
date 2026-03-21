using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Imaging;
using SF3.Models.Files.MPD;
using SF3.Models.Tables.MPD.Animation;

namespace SF3.Models.Structs.MPD.Animation {
    public class AnimationStruct : Struct, IMPD_Animation {
        private readonly int _textureIdAddr;
        private readonly int _widthAddr;
        private readonly int _heightAddr;
        private readonly int _frameTimerStartAddr;

        public AnimationStruct(IByteData data, int id, string name, int address, bool is32Bit, IMPD_File mpdFile)
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

            var frames = new List<AnimationFrame>();

            // This happens in Scn2 SARA23.MPD for some reason...
            // (It's totally a mistake in the file. The game crashes if you let it run for about 36 minutes!!)
            if (TextureIDRaw == _frameEndOffset)
                pos = Address + _bytesPerProperty;
            else if (TextureIDRaw != _textureEndId) {
                AnimationFrameTable = AnimationFrameTable.Create(data, "TexAnimFrames_" + id, pos, is32Bit, MPD_File, this);
                pos += AnimationFrameTable.SizeInBytesPlusTerminator;
            }

            Size = pos - Address;

            if (AnimationFrameTable == null)
                _frameByTimeFrame = new IMPD_AnimationFrame[0];
            else {
                _frameByTimeFrame = new IMPD_AnimationFrame[AnimationFrameTable.Sum(x => Math.Max(0, x.Duration))];
                var frameCounter = 0;
                foreach (var frame in AnimationFrameTable) {
                    for (var i = 0; i < frame.Duration; i++)
                        _frameByTimeFrame[frameCounter++] = frame;
                }
            }
        }

        public bool Is32Bit { get; }
        public IMPD_File MPD_File { get; }
        public int FramesAddress { get; }

        public IMPD_AnimationFrame GetFrame(int timeFrame) {
            return
                AnimationFrameTable.Count == 0 ? null :
                _frameByTimeFrame.Length == 0 ? AnimationFrameTable[0] :
                _frameByTimeFrame[MathHelpers.ActualMod(timeFrame + FrameTimerStart, _frameByTimeFrame.Length)];
        }

        public IMPD_AnimationFrame[] Frames => AnimationFrameTable.Rows;

        public uint TextureIDRaw {
            get => Data.GetData(_textureIdAddr, _bytesPerProperty);
            set => Data.SetData(_textureIdAddr, value, _bytesPerProperty);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_textureIdAddr), displayName: "Texture ID", displayOrder: 0, displayFormat: "X2")]
        public int TextureID {
            get => (int) (TextureIDRaw & 0xFF);
            set => TextureIDRaw = TextureIDRaw & ~0xFFu | (uint) value & 0xFF;
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_textureIdAddr), displayOrder: 0.1f)]
        public bool IsIndexed {
            get => (TextureIDRaw & 0x100) == 0x100;
            set => TextureIDRaw = TextureIDRaw & ~0x100u | (value ? 0x100u : 0);
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
        public int NumFrames => AnimationFrameTable?.Count ?? 0;

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 5)]
        public bool IsIgnored => MPD_File.IgnoredTextureTable?.ContainsTextureID(TextureID) == true;

        [BulkCopyRecurse]
        public AnimationFrameTable AnimationFrameTable { get; } = null;

        private readonly int _bytesPerProperty;
        private readonly uint _textureEndId;
        private readonly uint _frameEndOffset;

        private readonly IMPD_AnimationFrame[] _frameByTimeFrame;
    }
}
