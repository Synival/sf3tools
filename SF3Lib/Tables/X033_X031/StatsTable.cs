using SF3.RawEditors;
using SF3.Models.X033_X031;

namespace SF3.Tables.X033_X031 {
    public class StatsTable : Table<Stats> {
        public StatsTable(IRawEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Stats(FileEditor, id, name, address));

        public override int? MaxSize => 300;
    }
}
