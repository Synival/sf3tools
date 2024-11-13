using SF3.StreamEditors;
using SF3.Models.X013;

namespace SF3.Tables.X013 {
    public class SpecialTable : Table<Special> {
        public SpecialTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Special(FileEditor, id, name, address));

        public override int? MaxSize => 256;
    }
}
