using System;
using CommonLib.Attributes;
using SF3.Attributes;
using SF3.RawEditors;

namespace SF3.Models.MPD.TextureGroup {
    public class FrameModel : Model {
        public FrameModel(IRawEditor editor, int id, string name, int address, int texId, int width, int height, int groupId, int frameNum)
        : base(editor, id, name, address, 0x04) {
            TextureID = texId;
            Width     = width;
            Height    = height;
            GroupID   = groupId;
            FrameNum  = frameNum;
        }

        [ViewModelData(displayName: "Texture ID", displayOrder: 0, displayFormat: "X2")]
        public int TextureID { get; }

        [ViewModelData(displayName: "Width", displayOrder: 1)]
        public int Width { get; }

        [ViewModelData(displayName: "Height", displayOrder: 2)]
        public int Height { get; }

        [ViewModelData(displayName: "Group ID", displayOrder: 3, displayFormat: "X2")]
        public int GroupID { get; }

        public int FrameNum { get; }

        [ViewModelData(displayName: "Frame #", displayOrder: 4)]
        public string FrameNumStr => (FrameNum == 0) ? "" : FrameNum.ToString();

        [BulkCopy]
        [ViewModelData(displayName: "Texture Offset", displayOrder: 5, displayFormat: "X4")]
        public ushort CompressedTextureOffset {
            get => (ushort) Editor.GetWord(Address);
            set => Editor.SetWord(Address, value);
        }

        [BulkCopy]
        [ViewModelData(displayName: "Unknown", displayOrder: 6, displayFormat: "X4")]
        public ushort Unknown {
            get => (ushort) Editor.GetWord(Address + 2);
            set => Editor.SetWord(Address + 2, value);
        }
    }
}
