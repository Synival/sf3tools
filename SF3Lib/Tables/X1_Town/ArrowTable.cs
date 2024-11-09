using SF3.FileEditors;
using SF3.Models.X1_Town;

namespace SF3.Tables.X1_Town {
    public class ArrowTable : Table<Arrow> {
        public ArrowTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Arrow(FileEditor, id, name, address),
                (rows, prev, cur) => prev == null || prev.ArrowUnknown0 != 0xffff);

        public override int? MaxSize => 100;
    }
}
