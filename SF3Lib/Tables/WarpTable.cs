using System;
using System.Collections.Generic;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.FileEditors;
using SF3.Models;

namespace SF3.Tables {
    public class WarpTable : Table<Warp> {
        public WarpTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load() {
            var values = ResourceFile != null
                ? ResourceUtils.GetValueNameDictionaryFromXML(ResourceFile)
                : new Dictionary<int, string>();

            _rows = new Warp[0];
            int address = Address;
            Warp prevModel = null;
            for (var i = 0; prevModel == null || (prevModel.WarpType != 0x01 && prevModel.WarpType != 0xff); i++) {
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
