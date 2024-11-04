using SF3.FileEditors;
using SF3.Models;

namespace SF3.Tables {
    public class MagicBonusTable : Table<MagicBonus> {
        public MagicBonusTable(IByteEditor fileEditor, string resourceFile, int address, bool has32BitValues) : base(fileEditor, resourceFile, address) {
            Has32BitValues = has32BitValues;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new MagicBonus(FileEditor, id, name, address, Has32BitValues));

        public override int? MaxSize => 256;

        public bool Has32BitValues { get; }
    }
}
