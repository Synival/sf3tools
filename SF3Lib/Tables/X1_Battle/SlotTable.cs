using SF3.FileEditors;
using SF3.Models.X1_Battle;

namespace SF3.Tables.X1_Battle {
    public class SlotTable : Table<Slot> {
        public SlotTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Slot(FileEditor, id, name, address));

        public override int? MaxSize => 256;

    }
}
