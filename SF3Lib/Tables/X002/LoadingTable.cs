using SF3.FileEditors;
using SF3.Models.X002;

namespace SF3.Tables.X002 {
    public class LoadingTable : Table<Loading> {
        public LoadingTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Loading(FileEditor, id, name, address));

        public override int? MaxSize => 300;
    }
}
