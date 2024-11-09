using SF3.FileEditors;
using SF3.Models.X1_All;

namespace SF3.Tables.X1_All {
    public class TreasureTable : Table<Treasure> {
        public TreasureTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Treasure(FileEditor, id, name, address),
                (rows, prev, cur) => prev == null || prev.Searched != 0xffff);

        public override int? MaxSize => 255;
    }
}
