using SF3.Models.Structs.X1.Battle;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X1.Battle {
    public class AITable : Table<AI> {
        public AITable(IRawData editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new AI(Data, id, name, address));

        public override int? MaxSize => 130;
    }
}
