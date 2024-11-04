using SF3.FileEditors;
using SF3.Models;
using SF3.Types;

namespace SF3.Tables {
    public class SlotTable : Table<Slot> {
        public SlotTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Slot(FileEditor, id, name, address));

        public override int? MaxSize => 256;

    }
}
