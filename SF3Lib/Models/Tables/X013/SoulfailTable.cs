using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X013 {
    public class SoulfailTable : Table<Soulfail> {
        public SoulfailTable(IRawData editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Soulfail(Data, id, name, address));

        public override int? MaxSize => 1;
    }
}
