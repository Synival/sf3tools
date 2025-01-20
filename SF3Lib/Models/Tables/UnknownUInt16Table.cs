using System;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    public class UnknownUInt16Table : TerminatedTable<UnknownUInt16Struct> {
        protected UnknownUInt16Table(IByteData data, int address, int? count, int? readUntil)
        : base(data, address, count) {
            if (!count.HasValue && !readUntil.HasValue)
                throw new ArgumentNullException(nameof(count) + ", " + nameof(readUntil));

            ReadUntil = readUntil;
            FormatString = "X" + MaxSize.ToString().Length;
        }

        public static UnknownUInt16Table Create(IByteData data, int address, int? count, int? readUntil) {
            var newTable = new UnknownUInt16Table(data, address, count, readUntil);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        private bool ContinueReadingPred(Dictionary<int, UnknownUInt16Struct> currentRows, UnknownUInt16Struct newModel)
            => newModel.Value != ReadUntil.Value;

        public override bool Load() {
            var pred = ReadUntil.HasValue ? ContinueReadingPred : (ContinueReadingPredicate) null;
            return Load((id, address) => new UnknownUInt16Struct(Data, id, "Unknown UInt16 " + id.ToString(FormatString), address), pred, false);
        }

        public int? ReadUntil { get; }
        private string FormatString { get; }
    }
}
