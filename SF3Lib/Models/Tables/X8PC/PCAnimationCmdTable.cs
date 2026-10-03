using SF3.ByteData;
using SF3.Models.Structs.X8PC;
using SF3.Types;

namespace SF3.Models.Tables.X8PC {
    public class PCAnimationCmdTable : TerminatedTable<PCAnimationCmdStruct> {
        protected PCAnimationCmdTable(IByteData data, string name, int address, PCAnimationDefStruct animDef)
        : base(data, name, address, 0x02, maxSize: 100) {
            AnimDef = animDef;
        }

        public static PCAnimationCmdTable Create(IByteData data, string name, int address, PCAnimationDefStruct animDef)
            => Create(() => new PCAnimationCmdTable(data, name, address, animDef));

        public override bool Load()
            => Load((id, address) => new PCAnimationCmdStruct(Data, id, $"AnimCmd_{((PCAnimationType) AnimDef.AnimID)}_{id:D2}", address),
                (rows, row) => row.Data.GetUInt16(row.Address) != 0xffff,
                addEndModel: false
            );

        public PCAnimationDefStruct AnimDef { get; }
    }
}
