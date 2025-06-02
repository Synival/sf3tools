using System;
using SF3.ByteData;
using SF3.Models.Structs.X014;

namespace SF3.Models.Tables.X014 {
    public class CharacterBattleModelsSc1Table : ResourceTable<CharacterBattleModelsSc1> {
        protected CharacterBattleModelsSc1Table(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 0x100) {
        }

        public static CharacterBattleModelsSc1Table Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new CharacterBattleModelsSc1Table(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new CharacterBattleModelsSc1(Data, id, name, address));
    }
}
