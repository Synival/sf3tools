using SF3.FileEditors;
using SF3.Models;

namespace SF3.Tables {
    public class FriendshipExpTable : Table<FriendshipExp> {
        public FriendshipExpTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new FriendshipExp(FileEditor, id, name, address));

        public override int? MaxSize => 1;
    }
}
