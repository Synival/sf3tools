using System;
using System.Collections.Generic;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.RawEditors;
using SF3.Models.Shared;

namespace SF3.Tables.Shared {
    public class WarpTable : Table<Warp> {
        public WarpTable(IRawEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
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
                var newRow = new Warp(FileEditor, i, values.ContainsKey(i) ? values[i] : "WarpIndex" + i, address);
                address += newRow.Size;
                _rows = _rows.ExpandedWith(newRow);
                prevModel = newRow;
            }
            return true;
        }

        public override int? MaxSize => 1000;
    }
}
