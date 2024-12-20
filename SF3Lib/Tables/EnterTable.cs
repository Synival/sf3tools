using SF3.FileEditors;
using SF3.Models;

namespace SF3.Tables {
    public class EnterTable : Table<Enter> {
        public EnterTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Enter(FileEditor, id, name, address),
                (rows, prev, cur) => prev == null || prev.Entered != 0xffff);

        public override int? MaxSize => 100;
    }
}
