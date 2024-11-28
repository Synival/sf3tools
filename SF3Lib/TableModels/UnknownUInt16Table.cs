using System;
using System.Collections.Generic;
using System.Text;
using SF3.Models;
using SF3.RawEditors;

namespace SF3.TableModels {
    public class UnknownUInt16Table : Table<UnknownUInt16Model> {
        public UnknownUInt16Table(IRawEditor editor, int address, int count)
        : base(editor, address) {
            MaxSize = count;
            FormatString = "X" + MaxSize.ToString().Length;
        }

        public override bool Load() => LoadUntilMax((id, address)
            => new UnknownUInt16Model(Editor, id, "Unknown Int16 " + id.ToString(FormatString), address));

        public override int? MaxSize { get; }

        private string FormatString { get; }
    }
}
