using System;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class BlacksmithTable : TerminatedTable<Blacksmith> {
        protected BlacksmithTable(IByteData data, string name, int address)
        : base(data, name, address, terminatedBytes: 4, maxSize: 100) {
        }

        public static BlacksmithTable Create(IByteData data, string name, int address) {
            var newTable = new BlacksmithTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new Blacksmith(Data, id, "BlacksmithIndex" + id.ToString("D2"), addr),
                (rows, prevRow) => prevRow.MaterialItem != 0xFFFF,
                false
            );
        }

        public bool IsBattle { get; }
        public INameGetterContext NameGetterContext { get; }
    }
}
