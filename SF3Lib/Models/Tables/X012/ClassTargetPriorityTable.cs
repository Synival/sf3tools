using System;
using SF3.ByteData;
using SF3.Models.Structs.X012;

namespace SF3.Models.Tables.X012 {
    public class ClassTargetPriorityTable : FixedSizeTable<ClassTargetPriority> {
        protected ClassTargetPriorityTable(IByteData data, string name, int address) : base(data, name, address, 0x20) {
        }

        public static ClassTargetPriorityTable Create(IByteData data, string name, int address) {
            var newTable = new ClassTargetPriorityTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ClassTargetPriority(Data, id, "ClassTarget" + id.ToString("D2"), address));
    }
}
