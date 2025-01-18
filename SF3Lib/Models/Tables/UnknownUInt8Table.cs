using System;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    public class UnknownUInt8Table : Table<UnknownUInt8Struct> {
        protected UnknownUInt8Table(IByteData data, int address, int? count, int? readUntil)
        : base(data, address) {
            if (!count.HasValue && !readUntil.HasValue)
                throw new ArgumentNullException(nameof(count) + ", " + nameof(readUntil));

            MaxSize = count;
            ReadUntil = readUntil;

            FormatString = "X" + MaxSize.ToString().Length;
        }

        public static UnknownUInt8Table Create(IByteData data, int address, int? count, int? readUntil) {
            var newTable = new UnknownUInt8Table(data, address, count, readUntil);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        private bool ContinueReadingPred(Dictionary<int, UnknownUInt8Struct> currentRows, UnknownUInt8Struct newModel)
            => newModel.Value != ReadUntil.Value;

        public override bool Load() {
            var pred = ReadUntil.HasValue ? ContinueReadingPred : (ContinueReadingPredicate) null;
            return LoadUntilMax((id, address) => new UnknownUInt8Struct(Data, id, "Unknown UInt8 " + id.ToString(FormatString), address), pred, false);
        }

        public override int? MaxSize { get; }

        public int? ReadUntil { get; }

        private string FormatString { get; }
    }
}
