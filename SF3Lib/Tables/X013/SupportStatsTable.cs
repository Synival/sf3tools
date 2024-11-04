using SF3.FileEditors;
using SF3.Models.X013;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables.X013 {
    public class SupportStatsTable : Table<SupportStats> {
        public SupportStatsTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("X013StatList.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SupportStats(FileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 256;
    }
}
