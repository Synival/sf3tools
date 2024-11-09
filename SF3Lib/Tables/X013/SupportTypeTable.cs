using SF3.FileEditors;
using SF3.Models.X013;

namespace SF3.Tables.X013 {
    public class SupportTypeTable : Table<SupportType> {
        public SupportTypeTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SupportType(FileEditor, id, name, address));

        public override int? MaxSize => 120;
    }
}
