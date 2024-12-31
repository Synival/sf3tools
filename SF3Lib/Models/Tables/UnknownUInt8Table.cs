using System;
using System.Collections.Generic;
using System.Text;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Tables {
    public class UnknownUInt8Table : Table<UnknownUInt8Struct> {
        public UnknownUInt8Table(IByteData data, int address, int count)
        : base(data, address) {
            MaxSize = count;
            FormatString = "X" + MaxSize.ToString().Length;
        }

        public override bool Load() => LoadUntilMax((id, address)
            => new UnknownUInt8Struct(Data, id, "Unknown Int8 " + id.ToString(FormatString), address));

        public override int? MaxSize { get; }

        private string FormatString { get; }
    }
}
