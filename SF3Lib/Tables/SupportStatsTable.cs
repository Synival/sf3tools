using SF3.FileEditors;
using SF3.Models;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables {
    public class SupportStatsTable : Table<SupportStats> {
        public SupportStatsTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SupportStats(FileEditor, id, name, address));

        public override int? MaxSize => 256;
    }
}
