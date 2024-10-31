using SF3.FileEditors;
using SF3.Models;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables {
    public class WeaponRankTable : Table<WeaponRank> {
        public override int? MaxSize => 5;

        public WeaponRankTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("WeaponRankList.xml");
            Address = address;
        }

        public override string ResourceFile { get; }
        public override int Address { get; }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new WeaponRank(FileEditor, id, name, address));
    }
}
