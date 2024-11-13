using SF3.RawEditors;
using SF3.Models.Shared;

namespace SF3.Tables.Shared {
    public class TileMovementTable : Table<TileMovement> {
        public TileMovementTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new TileMovement(Editor, id, name, address));

        public override int? MaxSize => 31;
    }
}
