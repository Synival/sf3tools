using SF3.RawEditors;
using SF3.Models.X002;

namespace SF3.Tables.X002 {
    public class LoadingTable : Table<Loading> {
        public LoadingTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Loading(Editor, id, name, address));

        public override int? MaxSize => 300;
    }
}
