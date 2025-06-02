using System;
using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class AttackAnimationIdTable : ResourceTable<AttackAnimationIdModel> {
        protected AttackAnimationIdTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 0x100) {
        }

        public static AttackAnimationIdTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new AttackAnimationIdTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new AttackAnimationIdModel(Data, id, name, address));
    }
}
