using System;
using System.Collections.Generic;
using System.Text;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Tables {
    public class UnknownUInt8Table : Table<UnknownUInt8Struct> {
        protected UnknownUInt8Table(IByteData data, int address, int count)
        : base(data, address) {
            MaxSize = count;
            FormatString = "X" + MaxSize.ToString().Length;
        }

        public static UnknownUInt8Table Create(IByteData data, int address, int count) {
            var newTable = new UnknownUInt8Table(data, address, count);
            newTable.Load();
            return newTable;
        }

        public override bool Load() => LoadUntilMax((id, address)
            => new UnknownUInt8Struct(Data, id, "Unknown Int8 " + id.ToString(FormatString), address));

        public override int? MaxSize { get; }

        private string FormatString { get; }
    }
}
