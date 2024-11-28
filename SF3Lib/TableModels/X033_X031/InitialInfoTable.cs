using SF3.RawEditors;
using SF3.Structs.X033_X031;

namespace SF3.TableModels.X033_X031 {
    public class InitialInfoTable : Table<InitialInfo> {
        public InitialInfoTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new InitialInfo(Editor, id, name, address));

        public override int? MaxSize => 100;
    }
}
