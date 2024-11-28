using System;
using System.Collections.Generic;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.Models.Structs.Shared;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.Shared {
    public class WarpTable : Table<Warp> {
        public WarpTable(IRawData editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load() {
            var values = ResourceFile != null
                ? ResourceUtils.GetValueNameDictionaryFromXML(ResourceFile)
                : new Dictionary<int, string>();

            _rows = new Warp[0];
            var address = Address;
            Warp prevModel = null;
            for (var i = 0; prevModel == null || prevModel.WarpType != 0x01 && prevModel.WarpType != 0xff; i++) {
                if (i == MaxSize)
                    throw new IndexOutOfRangeException();
                var newRow = new Warp(Editor, i, values.ContainsKey(i) ? values[i] : "WarpIndex" + i, address);
                address += newRow.Size;
                _rows = _rows.ExpandedWith(newRow);
                prevModel = newRow;
            }
            return true;
        }

        public override int? MaxSize => 1000;
    }
}