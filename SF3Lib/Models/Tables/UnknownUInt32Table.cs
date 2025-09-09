using System;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    public class UnknownUInt32Table : TerminatedTable<UnknownUInt32Struct> {
        protected UnknownUInt32Table(IByteData data, string name, int address, int? count, int? readUntil)
        : base(data, name, address, readUntil.HasValue ? 4 : 0, count) {
            if (!count.HasValue && !readUntil.HasValue)
                throw new ArgumentNullException(nameof(count) + ", " + nameof(readUntil));

            ReadUntil = readUntil;
            FormatString = "X" + MaxSize.ToString().Length;
        }

        public static UnknownUInt32Table Create(IByteData data, string name, int address, int? count, int? readUntil)
            => Create(() => new UnknownUInt32Table(data, name, address, count, readUntil));

        private bool ContinueReadingPred(Dictionary<int, UnknownUInt32Struct> currentRows, UnknownUInt32Struct newModel)
            => newModel.Value != ReadUntil.Value;

        public override bool Load() {
            var pred = ReadUntil.HasValue ? ContinueReadingPred : (ContinueReadingPredicate) null;
            return Load((id, address) => new UnknownUInt32Struct(Data, id, "Unknown UInt32 " + id.ToString(FormatString), address), pred, false);
        }

        public int? ReadUntil { get; }
        private string FormatString { get; }
    }
}
