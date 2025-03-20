using System;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class WarpTable : TerminatedTable<Warp> {
        protected WarpTable(IByteData data, string name, int address, bool isBattle, INameGetterContext nameGetterContext)
        : base(data, name, address, terminatedBytes: 4, maxSize: 1000) {
            IsBattle = isBattle;
            NameGetterContext = nameGetterContext;
        }

        public static WarpTable Create(IByteData data, string name, int address, bool isBattle, INameGetterContext nameGetterContext) {
            var newTable = new WarpTable(data, name, address, isBattle, nameGetterContext);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            Warp prevWarp = null;
            return Load(
                (id, addr) => new Warp(Data, id, "WarpIndex" + id.ToString("D3"), addr, IsBattle, prevWarp, NameGetterContext),
                (rows, prevRow) => {
                    prevWarp = prevRow;
                    return (prevRow.LoadID != 0x1FF);
                },
                false
            );
        }

        public bool IsBattle { get; }
        public INameGetterContext NameGetterContext { get; }
    }
}
