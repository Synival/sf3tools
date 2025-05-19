using System;
using SF3.ByteData;
using SF3.Models.Structs.X014;

namespace SF3.Models.Tables.X014 {
    public class CharacterBattleModelSc2Table : ResourceTable<CharacterBattleSc2Model> {
        protected CharacterBattleModelSc2Table(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 0x100) {
        }

        public static CharacterBattleModelSc2Table Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new CharacterBattleModelSc2Table(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new CharacterBattleSc2Model(Data, id, name, address));
    }
}
