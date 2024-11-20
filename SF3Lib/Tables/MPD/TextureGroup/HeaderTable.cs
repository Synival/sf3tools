using System;
using System.Collections.Generic;
using System.Text;
using SF3.Models.MPD.TextureGroup;
using SF3.RawEditors;

namespace SF3.Tables.MPD.TextureGroup {
    public class HeaderTable : Table<HeaderModel> {
        public HeaderTable(IRawEditor editor, int address) : base(editor, address) {
        }

        public override bool Load() {
            return LoadUntilMax(
                (id, address) => {
                    var atEnd = (uint) Editor.GetWord(address) == 0xFFFF;
                    return new HeaderModel(Editor, id, atEnd ? "--" : "TexGroup" + id, address);
                },
                (currentRows, prevModel, currentModel) => prevModel?.TextureID != 0xFFFF);
        }
    }
}
