using SF3.RawEditors;
using SF3.Models.X002;

namespace SF3.TableModels.X002 {
    public class LoadedOverrideTable : Table<LoadedOverride> {
        public LoadedOverrideTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new LoadedOverride(Editor, id, name, address));

        public override int? MaxSize => 300;
    }
}
