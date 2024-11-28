﻿using SF3.Structs.MPD;
using SF3.RawEditors;

namespace SF3.TableModels.MPD {
    public class TextureAnimationTable : Table<TextureAnimationModel> {
        public TextureAnimationTable(IRawEditor editor, int address, bool is32Bit) : base(editor, address) {
            Is32Bit = is32Bit;
            _textureEndId = is32Bit ? 0xFFFF_FFFF : 0xFFFF;
        }

        public override bool Load() {
            return LoadUntilMax(
                (id, address) => {
                    var textureId = Editor.GetData(address, Is32Bit ? 4 : 2);
                    var atEnd = textureId == _textureEndId;
                    return new TextureAnimationModel(Editor, id, atEnd ? "--" : "TexAnim" + id, address, Is32Bit);
                },
                (currentRows, model) => model.TextureID != _textureEndId);
        }

        public bool Is32Bit { get; }

        private uint _textureEndId;
    }
}
