using System;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    public class UnknownUInt8Table : Table<UnknownUInt8Struct> {
        protected UnknownUInt8Table(IByteData data, int address, int count)
        : base(data, address) {
            MaxSize = count;
            FormatString = "X" + MaxSize.ToString().Length;
        }

        public static UnknownUInt8Table Create(IByteData data, int address, int count) {
            var newTable = new UnknownUInt8Table(data, address, count);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() => LoadUntilMax((id, address)
            => new UnknownUInt8Struct(Data, id, "Unknown Int8 " + id.ToString(FormatString), address));

        public override int? MaxSize { get; }

        private string FormatString { get; }
    }
}
