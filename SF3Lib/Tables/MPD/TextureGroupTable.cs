using System;
using System.Collections.Generic;
using System.Text;
using SF3.Models.MPD;
using SF3.RawEditors;

namespace SF3.Tables.MPD {
    public class TextureGroupTable : Table<TextureGroupModel> {
        public TextureGroupTable(IRawEditor editor, int address) : base(editor, address) {
        }

        public override bool Load() {
            return LoadUntilMax(
                (id, address) => {
                    var atEnd = ((uint) Editor.GetWord(address)) == 0xFFFF;
                    return new TextureGroupModel(Editor, id, atEnd ? "--" : ("Texture Group " + id), address);
                },
                (currentRows, prevModel, currentModel) => prevModel?.TextureID != 0xFFFF);
        }
    }
}
