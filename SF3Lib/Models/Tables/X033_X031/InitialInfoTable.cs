using SF3.Models.Structs.X033_X031;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X033_X031 {
    public class InitialInfoTable : Table<InitialInfo> {
        public InitialInfoTable(IRawData editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new InitialInfo(Editor, id, name, address));

        public override int? MaxSize => 100;
    }
}
