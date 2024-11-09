using SF3.FileEditors;
using SF3.Models.X002;

namespace SF3.Tables.X002 {
    public class StatBoostTable : Table<StatBoost> {
        public StatBoostTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new StatBoost(FileEditor, id, name, address));

        public override int? MaxSize => 300;
    }
}
