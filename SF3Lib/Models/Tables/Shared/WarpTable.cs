using System;
using System.Collections.Generic;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class WarpTable : ResourceTable<Warp> {
        protected WarpTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 1000) {
        }

        public static WarpTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new WarpTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
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
                var newRow = new Warp(Data, i, values.ContainsKey(i) ? values[i] : "WarpIndex" + i, address);
                address += newRow.Size;
                _rows = _rows.ExpandedWith(newRow);
                prevModel = newRow;
            }
            return true;
        }
    }
}
