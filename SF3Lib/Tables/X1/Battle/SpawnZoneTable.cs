using SF3.RawEditors;
using SF3.Models.X1.Battle;

namespace SF3.Tables.X1.Battle {
    public class SpawnZoneTable : Table<SpawnZone> {
        public SpawnZoneTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpawnZone(Editor, id, name, address));

        public override int? MaxSize => 30;
    }
}
