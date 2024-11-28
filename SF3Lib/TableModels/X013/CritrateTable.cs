using SF3.RawEditors;
using SF3.Models.X013;

namespace SF3.TableModels.X013 {
    public class CritrateTable : Table<Critrate> {
        public CritrateTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Critrate(Editor, id, name, address));

        public override int? MaxSize => 3;
    }
}
