using SF3.RawEditors;
using SF3.Structs.X033_X031;

namespace SF3.TableModels.X033_X031 {
    public class StatsTable : Table<Stats> {
        public StatsTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Stats(Editor, id, name, address));

        public override int? MaxSize => 300;
    }
}
