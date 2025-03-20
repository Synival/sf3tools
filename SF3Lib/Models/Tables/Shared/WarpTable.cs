using System;
using System.Collections.Generic;
using CommonLib.Extensions;
using CommonLib.NamedValues;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class WarpTable : TerminatedTable<Warp> {
        protected WarpTable(IByteData data, string name, int address, INameGetterContext nameGetterContext)
        : base(data, name, address, terminatedBytes: 4, maxSize: 1000) {
            NameGetterContext = nameGetterContext;
        }

        public static WarpTable Create(IByteData data, string name, int address, INameGetterContext nameGetterContext) {
            var newTable = new WarpTable(data, name, address, nameGetterContext);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            Warp prevWarp = null;
            return Load(
                (id, addr) => new Warp(Data, id, "WarpIndex" + id.ToString("D3"), addr, prevWarp, NameGetterContext),
                (rows, prevRow) => {
                    prevWarp = prevRow;
                    return (prevRow.LoadID != 0x1FF);
                },
                false
            );
        }

        public INameGetterContext NameGetterContext { get; }
    }
}
