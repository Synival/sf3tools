using System.Collections.Generic;
using SF3.Models.MPD;
using SF3.Models.MPD.TextureAnimation;
using SF3.RawEditors;

namespace SF3.Tables.MPD.TextureAnimation {
    public class FrameTable : Table<FrameModel> {
        public FrameTable(IRawEditor editor, int address, bool is32Bit, IEnumerable<TextureAnimationModel> textures) : base(editor, address) {
            Is32Bit = is32Bit;
            Textures = textures;
            _textureIdEnd = Is32Bit ? 0xFFFF_FFFF : 0xFFFF;
        }

        public override bool Load() {
            var frameModels = new List<FrameModel>();

            int id = 0;
            foreach (var tex in Textures) {
                if (tex.TextureID == _textureIdEnd)
                    break;

                int addr = tex.FramesAddress;
                for (var i = 0; i < tex.NumFrames; i++) {
                    var newModel = new FrameModel(Editor, id++, tex.Name + "_" + (i + 1), addr, Is32Bit, (int) tex.TextureID, (int) tex.Width, (int) tex.Height, tex.ID, i + 1);
                    frameModels.Add(newModel);
                    addr += newModel.Size;
                }
                frameModels.Add(new FrameModel(Editor, id++, "--", addr, Is32Bit, (int) tex.TextureID, (int) tex.Width, (int) tex.Height, tex.ID, 0));
            }

            _rows = frameModels.ToArray();
            return true;
        }

        public bool Is32Bit { get; }
        public IEnumerable<TextureAnimationModel> Textures { get; }

        private uint _textureIdEnd;
    }
}
