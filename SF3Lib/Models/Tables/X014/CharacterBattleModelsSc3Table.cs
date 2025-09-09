using SF3.ByteData;
using SF3.Models.Structs.X014;

namespace SF3.Models.Tables.X014 {
    public class CharacterBattleModelsSc3Table : ResourceTable<CharacterBattleModelsSc3> {
        protected CharacterBattleModelsSc3Table(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 0x100) {
        }

        public static CharacterBattleModelsSc3Table Create(IByteData data, string name, string resourceFile, int address)
            => CreateBase(() => new CharacterBattleModelsSc3Table(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new CharacterBattleModelsSc3(Data, id, name, address));
    }
}
