using System;
using System.Collections.Generic;
using System.Text;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Tables {
    public class UnknownUInt16Table : Table<UnknownUInt16Struct> {
        protected UnknownUInt16Table(IByteData data, int address, int count)
        : base(data, address) {
            MaxSize = count;
            FormatString = "X" + MaxSize.ToString().Length;
        }

        public static UnknownUInt16Table Create(IByteData data, int address, int count) {
            var newTable = new UnknownUInt16Table(data, address, count);
            newTable.Load();
            return newTable;
        }

        public override bool Load() => LoadUntilMax((id, address)
            => new UnknownUInt16Struct(Data, id, "Unknown Int16 " + id.ToString(FormatString), address));

        public override int? MaxSize { get; }

        private string FormatString { get; }
    }
}
