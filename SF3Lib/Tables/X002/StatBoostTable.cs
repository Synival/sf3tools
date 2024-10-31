using SF3.FileEditors;
using SF3.Models.X002;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables.X002 {
    public class StatBoostTable : Table<StatBoost> {
        public override int? MaxSize => 300;

        public StatBoostTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("X002StatList.xml");
            Address = address;
        }

        public override string ResourceFile { get; }
        public override int Address { get; }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new StatBoost(FileEditor, id, name, address));
    }
}
