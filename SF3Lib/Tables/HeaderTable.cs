using SF3.FileEditors;
using SF3.Models;

namespace SF3.Tables {
    public class HeaderTable : Table<Header> {
        public HeaderTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Header(FileEditor, id, name, address));

        public override int? MaxSize => 31;
    }
}
