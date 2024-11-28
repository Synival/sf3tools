using System;
using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.MPD.TextureAnimation {
    public class FrameModel : Struct {
        private readonly int _compressedTextureOffsetAddress;
        private readonly int _unknownAddress;

        public FrameModel(IRawData editor, int id, string name, int address, bool is32Bit, int texId, int width, int height, int texAnimId, int frameNum)
        : base(editor, id, name, address, is32Bit ? 0x08 : 0x04) {
            Is32Bit   = is32Bit;
            TextureID = texId;
            Width     = width;
            Height    = height;
            texAnimID = texAnimId;
            FrameNum  = frameNum;

            _bytesPerProperty = is32Bit ? 0x04 : 0x02;

            _compressedTextureOffsetAddress = Address + 0 * _bytesPerProperty;
            _unknownAddress                 = Address + 1 * _bytesPerProperty;
        }

        public bool Is32Bit { get; }

        [TableViewModelColumn(displayName: "Texture ID", displayOrder: 0, displayFormat: "X2")]
        public int TextureID { get; }

        [TableViewModelColumn(displayName: "Width", displayOrder: 1)]
        public int Width { get; }

        [TableViewModelColumn(displayName: "Height", displayOrder: 2)]
        public int Height { get; }

        [TableViewModelColumn(displayName: "Tex. Anim ID", displayOrder: 3, displayFormat: "X2")]
        public int texAnimID { get; }

        public int FrameNum { get; }

        [TableViewModelColumn(displayName: "Frame #", displayOrder: 4)]
        public string FrameNumStr => FrameNum == 0 ? "" : FrameNum.ToString();

        [BulkCopy]
        [TableViewModelColumn(displayName: "Texture Offset", displayOrder: 5, displayFormat: "X4")]
        public uint CompressedTextureOffset {
            get => Editor.GetData(_compressedTextureOffsetAddress, _bytesPerProperty);
            set => Editor.SetData(_compressedTextureOffsetAddress, value, _bytesPerProperty);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Unknown", displayOrder: 6, displayFormat: "X4")]
        public uint Unknown {
            get => Editor.GetData(_unknownAddress, _bytesPerProperty);
            set => Editor.SetData(_unknownAddress, value, _bytesPerProperty);
        }

        private readonly int _bytesPerProperty;
    }
}
