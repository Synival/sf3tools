using SF3.FileEditors;
using SF3.Models;

namespace SF3.Tables {
    public class SoulmateTable : Table<Soulmate> {
        public SoulmateTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Soulmate(FileEditor, id, name, address));

        public override int? MaxSize => 1771;
    }
}
