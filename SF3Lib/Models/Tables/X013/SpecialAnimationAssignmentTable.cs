using System;
using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class SpecialAnimationAssignmentTable : FixedSizeTable<SpecialAnimationAssignment> {
        protected SpecialAnimationAssignmentTable(IByteData data, string name, int address, int size) : base(data, name, address, size) {
        }

        public static SpecialAnimationAssignmentTable Create(IByteData data, string name, int address, int size) {
            var newTable = new SpecialAnimationAssignmentTable(data, name, address, size);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new SpecialAnimationAssignment(Data, id, $"SpecialAnimation{id:D2}", address));
    }
}
