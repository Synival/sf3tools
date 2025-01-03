using System;
using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class FriendshipExpTable : Table<FriendshipExp> {
        protected FriendshipExpTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static FriendshipExpTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new FriendshipExpTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new FriendshipExp(Data, id, name, address));

        public override int? MaxSize => 1;
    }
}
