using System;
using System.Collections.Generic;
using System.Text;
using SF3.FileEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class HeaderTable : Table<Header> {
        public HeaderTable(IByteEditor fileEditor, int address) : base(fileEditor, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new Header(FileEditor, id, "Header", address));

        public override int? MaxSize => 1;
    }
}
