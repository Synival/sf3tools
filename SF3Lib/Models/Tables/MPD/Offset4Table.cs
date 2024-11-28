﻿using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.MPD {
    public class Offset4Table : Table<Offset4Model> {
        public Offset4Table(IRawData editor, int address) : base(editor, address) {
        }

        public override bool Load() {
            return LoadUntilMax(
                (id, address) => {
                    var atEnd = (uint) Editor.GetDouble(address) == 0xFFFF_FFFF;
                    return new Offset4Model(Editor, id, atEnd ? "--" : "Row " + id, address);
                },
                (currentRows, model) => model.Unknown1 != 0xFFFF_FFFF);
        }
    }
}