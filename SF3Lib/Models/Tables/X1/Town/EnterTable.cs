using SF3.Models.Structs.X1.Town;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X1.Town {
    public class EnterTable : Table<Enter> {
        public EnterTable(IRawData editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Enter(Data, id, name, address),
                (rows, models) => models.Entered != 0xFFFF);

        public override int? MaxSize => 100;
    }
}
