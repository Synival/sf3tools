using CommonLib.Attributes;
using SF3.Attributes;
using SF3.RawEditors;

namespace SF3.Models.MPD.TextureGroup {
    public class FrameModel : Model {
        public FrameModel(IRawEditor editor, int id, string name, int address, int texId, int frameNum)
        : base(editor, id, name, address, 0x04) {
            TextureID = texId;
            FrameNum = frameNum;
        }

        [ViewModelData(displayName: "Texture ID", displayOrder: 0, displayFormat: "X2")]
        public int TextureID { get; }

        public int FrameNum { get; }

        [ViewModelData(displayName: "Frame #", displayOrder: 1)]
        public string FrameNumStr => (FrameNum == 0) ? "" : FrameNum.ToString();

        [BulkCopy]
        [ViewModelData(displayName: "Compressed Texture Offset", displayOrder: 2, displayFormat: "X4")]
        public ushort CompressedTextureOffset {
            get => (ushort) Editor.GetWord(Address);
            set => Editor.SetWord(Address, value);
        }

        [BulkCopy]
        [ViewModelData(displayName: "Unknown", displayOrder: 3, displayFormat: "X4")]
        public ushort Unknown {
            get => (ushort) Editor.GetWord(Address + 2);
            set => Editor.SetWord(Address + 2, value);
        }
    }
}
