using SF3.FileEditors;
using SF3.Models;

namespace SF3.Tables {
    public class TileMovementTable : Table<TileMovement> {
        public TileMovementTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new TileMovement(FileEditor, id, name, address));

        public override int? MaxSize => 31;
    }
}
