using System;
using CommonLib.Attributes;
using SF3.RawEditors;

namespace SF3.Models.MPD.TextureAnimation {
    public class FrameModel : Model {
        private readonly int _compressedTextureOffsetAddress;
        private readonly int _unknownAddress;

        public FrameModel(IRawEditor editor, int id, string name, int address, bool is32Bit, int texId, int width, int height, int texAnimId, int frameNum)
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

        [DataViewModelColumn(displayName: "Texture ID", displayOrder: 0, displayFormat: "X2")]
        public int TextureID { get; }

        [DataViewModelColumn(displayName: "Width", displayOrder: 1)]
        public int Width { get; }

        [DataViewModelColumn(displayName: "Height", displayOrder: 2)]
        public int Height { get; }

        [DataViewModelColumn(displayName: "Tex. Anim ID", displayOrder: 3, displayFormat: "X2")]
        public int texAnimID { get; }

        public int FrameNum { get; }

        [DataViewModelColumn(displayName: "Frame #", displayOrder: 4)]
        public string FrameNumStr => (FrameNum == 0) ? "" : FrameNum.ToString();

        [BulkCopy]
        [DataViewModelColumn(displayName: "Texture Offset", displayOrder: 5, displayFormat: "X4")]
        public uint CompressedTextureOffset {
            get => Editor.GetData(_compressedTextureOffsetAddress, _bytesPerProperty);
            set => Editor.SetData(_compressedTextureOffsetAddress, value, _bytesPerProperty);
        }

        [BulkCopy]
        [DataViewModelColumn(displayName: "Unknown", displayOrder: 6, displayFormat: "X4")]
        public uint Unknown {
            get => Editor.GetData(_unknownAddress, _bytesPerProperty);
            set => Editor.SetData(_unknownAddress, value, _bytesPerProperty);
        }

        private readonly int _bytesPerProperty;
    }
}
