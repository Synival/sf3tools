using SF3.RawEditors;
using SF3.Models.X013;

namespace SF3.Tables.X013 {
    public class FriendshipExpTable : Table<FriendshipExp> {
        public FriendshipExpTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new FriendshipExp(FileEditor, id, name, address));

        public override int? MaxSize => 1;
    }
}
