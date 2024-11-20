using System.Collections.Generic;
using SF3.Models.MPD.TextureGroup;
using SF3.RawEditors;

namespace SF3.Tables.MPD.TextureGroup {
    public class FrameTable : Table<FrameModel> {
        public FrameTable(IRawEditor editor, int address, IEnumerable<HeaderModel> textures) : base(editor, address) {
            Textures = textures;
        }

        public override bool Load() {
            var frameModels = new List<FrameModel>();

            int id = 0;
            foreach (var tex in Textures) {
                if (tex.TextureID == 0xFFFF)
                    break;

                int addr = tex.FramesAddress;
                for (var i = 0; i < tex.NumFrames; i++) {
                    var newModel = new FrameModel(Editor, id++, "Group" + tex.ID + " Frame" + (i + 1), addr, tex.TextureID, i + 1);
                    frameModels.Add(newModel);
                    addr += newModel.Size;
                }
                frameModels.Add(new FrameModel(Editor, id++, "--", addr, tex.TextureID, 0));
            }

            _rows = frameModels.ToArray();
            return true;
        }

        public IEnumerable<HeaderModel> Textures { get; }
    }
}
