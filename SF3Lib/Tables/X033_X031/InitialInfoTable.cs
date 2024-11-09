using SF3.FileEditors;
using SF3.Models.X033_X031;

namespace SF3.Tables.X033_X031 {
    public class InitialInfoTable : Table<InitialInfo> {
        public InitialInfoTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new InitialInfo(FileEditor, id, name, address));

        public override int? MaxSize => 100;
    }
}
