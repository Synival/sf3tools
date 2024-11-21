using System;
using System.Collections.Generic;
using System.Text;
using SF3.Models.MPD;
using SF3.RawEditors;

namespace SF3.Tables.MPD {
    public class Offset4Table : Table<Offset4Model> {
        public Offset4Table(IRawEditor editor, int address) : base(editor, address) {
        }

        public override bool Load() {
            return LoadUntilMax(
                (id, address) => {
                    var atEnd = ((uint) Editor.GetDouble(address)) == 0xFFFF_FFFF;
                    return new Offset4Model(Editor, id, atEnd ? "--" : ("Row " + id), address);
                },
                (currentRows, model) => model.Value1 != 0xFFFF_FFFF);
        }
    }
}
