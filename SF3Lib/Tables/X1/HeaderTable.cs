using SF3.FileEditors;
using SF3.Models.X1;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables.X1 {
    public class HeaderTable : Table<Header> {
        public HeaderTable(ISF3FileEditor fileEditor, int address, int battlePointersTableOffset) : base(fileEditor) {
            ResourceFile = ResourceFile("X1Top.xml");
            Address = address;
            BattlePointersTableOffset = battlePointersTableOffset;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Header(FileEditor, id, name, address, BattlePointersTableOffset));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 31;

        public int BattlePointersTableOffset { get; }
    }
}
