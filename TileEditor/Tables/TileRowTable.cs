using SF3.FileEditors;
using SF3.Tables;
using SF3.TileEditor.Models;

namespace SF3.TileEditor.Tables {
    public class TileRowTable : Table<TileRow> {
        public TileRowTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new TileRow(FileEditor, id, name, address));

        public override int? MaxSize => 64;
    }
}
