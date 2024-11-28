using System.Collections.Generic;
using SF3.Models.Structs.MPD;
using SF3.Models.Structs.MPD.TextureAnimation;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.MPD.TextureAnimation {
    public class FrameTable : Table<FrameModel> {
        public FrameTable(IRawData editor, int address, bool is32Bit, IEnumerable<TextureAnimationModel> textures) : base(editor, address) {
            Is32Bit = is32Bit;
            Textures = textures;
            _textureIdEnd = Is32Bit ? 0xFFFF_FFFF : 0xFFFF;
        }

        public override bool Load() {
            var frameModels = new List<FrameModel>();

            var id = 0;
            foreach (var tex in Textures) {
                if (tex.TextureID == _textureIdEnd)
                    break;

                var addr = tex.FramesAddress;
                for (var i = 0; i < tex.NumFrames; i++) {
                    var newModel = new FrameModel(Data, id++, tex.Name + "_" + (i + 1), addr, Is32Bit, (int) tex.TextureID, (int) tex.Width, (int) tex.Height, tex.ID, i + 1);
                    frameModels.Add(newModel);
                    addr += newModel.Size;
                }
                frameModels.Add(new FrameModel(Data, id++, "--", addr, Is32Bit, (int) tex.TextureID, (int) tex.Width, (int) tex.Height, tex.ID, 0));
            }

            _rows = frameModels.ToArray();
            return true;
        }

        public bool Is32Bit { get; }
        public IEnumerable<TextureAnimationModel> Textures { get; }

        private uint _textureIdEnd;
    }
}
