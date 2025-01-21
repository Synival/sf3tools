using System;
using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class FriendshipExpTable : ResourceTable<FriendshipExp> {
        protected FriendshipExpTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 1) {
        }

        public static FriendshipExpTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new FriendshipExpTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new FriendshipExp(Data, id, name, address));
    }
}
