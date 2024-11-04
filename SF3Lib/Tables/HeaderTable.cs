using SF3.FileEditors;
using SF3.Models;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables {
    public class HeaderTable : Table<Header> {
        public HeaderTable(IByteEditor fileEditor, string resourceFile, int address, int battlePointersTableOffset) : base(fileEditor, resourceFile, address) {
            BattlePointersTableOffset = battlePointersTableOffset;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Header(FileEditor, id, name, address, BattlePointersTableOffset));

        public override int? MaxSize => 31;

        public int BattlePointersTableOffset { get; }
    }
}
