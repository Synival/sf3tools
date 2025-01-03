using System;
using System.Collections.Generic;
using System.Text;
using SF3.Models.Structs;
using SF3.ByteData;

namespace SF3.Models.Tables {
    public class UnknownUInt32Table : Table<UnknownUInt32Struct> {
        protected UnknownUInt32Table(IByteData data, int address, int count)
        : base(data, address) {
            MaxSize = count;
            FormatString = "X" + MaxSize.ToString().Length;
        }

        public static UnknownUInt32Table Create(IByteData data, int address, int count) {
            var newTable = new UnknownUInt32Table(data, address, count);
            newTable.Load();
            return newTable;
        }

        public override bool Load() => LoadUntilMax((id, address)
            => new UnknownUInt32Struct(Data, id, "Unknown UInt32 " + id.ToString(FormatString), address));

        public override int? MaxSize { get; }

        private string FormatString { get; }
    }
}
