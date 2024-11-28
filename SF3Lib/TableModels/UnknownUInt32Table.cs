using System;
using System.Collections.Generic;
using System.Text;
using SF3.Structs;
using SF3.RawEditors;

namespace SF3.TableModels {
    public class UnknownUInt32Table : Table<UnknownUInt32Struct> {
        public UnknownUInt32Table(IRawEditor editor, int address, int count)
        : base(editor, address) {
            MaxSize = count;
            FormatString = "X" + MaxSize.ToString().Length;
        }

        public override bool Load() => LoadUntilMax((id, address)
            => new UnknownUInt32Struct(Editor, id, "Unknown UInt32 " + id.ToString(FormatString), address));

        public override int? MaxSize { get; }

        private string FormatString { get; }
    }
}
