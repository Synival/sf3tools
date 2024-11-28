using SF3.RawEditors;
using SF3.Structs.X013;

namespace SF3.TableModels.X013 {
    public class SupportTypeTable : Table<SupportType> {
        public SupportTypeTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SupportType(Editor, id, name, address));

        public override int? MaxSize => 120;
    }
}
