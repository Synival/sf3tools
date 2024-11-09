using SF3.FileEditors;
using SF3.Models.X1_Battle;

namespace SF3.Tables.X1_Battle {
    public class AITable : Table<AI> {
        public AITable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new AI(FileEditor, id, name, address));

        public override int? MaxSize => 130;
    }
}