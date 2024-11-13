using SF3.StreamEditors;
using SF3.Models.X1.Battle;

namespace SF3.Tables.X1.Battle {
    public class HeaderTable : Table<Header> {
        public HeaderTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Header(FileEditor, id, name, address));

        public override int? MaxSize => 31;
    }
}
