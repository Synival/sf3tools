using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.RawEditors;

namespace SF3.Models.Tables.X013 {
    public class SupportTypeTable : Table<SupportType> {
        public SupportTypeTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SupportType(Editor, id, name, address));

        public override int? MaxSize => 120;
    }
}
