using System;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    public class UnknownUInt8Table : TerminatedTable<UnknownUInt8Struct> {
        protected UnknownUInt8Table(IByteData data, string name, int address, int? count, int? readUntil)
        : base(data, name, address, readUntil.HasValue ? 1 : 0, count) {
            if (!count.HasValue && !readUntil.HasValue)
                throw new ArgumentNullException(nameof(count) + ", " + nameof(readUntil));

            ReadUntil = readUntil;
            FormatString = "X" + MaxSize.ToString().Length;
        }

        public static UnknownUInt8Table Create(IByteData data, string name, int address, int? count, int? readUntil)
            => CreateBase(() => new UnknownUInt8Table(data, name, address, count, readUntil));

        private bool ContinueReadingPred(Dictionary<int, UnknownUInt8Struct> currentRows, UnknownUInt8Struct newModel)
            => newModel.Value != ReadUntil.Value;

        public override bool Load() {
            var pred = ReadUntil.HasValue ? ContinueReadingPred : (ContinueReadingPredicate) null;
            return Load((id, address) => new UnknownUInt8Struct(Data, id, "Unknown UInt8 " + id.ToString(FormatString), address), pred, false);
        }

        public int? ReadUntil { get; }
        private string FormatString { get; }
    }
}
