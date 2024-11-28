using SF3.RawEditors;
using SF3.Structs.X013;

namespace SF3.TableModels.X013 {
    public class FriendshipExpTable : Table<FriendshipExp> {
        public FriendshipExpTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new FriendshipExp(Editor, id, name, address));

        public override int? MaxSize => 1;
    }
}
