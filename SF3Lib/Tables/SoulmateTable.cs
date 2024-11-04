using SF3.FileEditors;
using SF3.Models;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables {
    public class SoulmateTable : Table<Soulmate> {
        public SoulmateTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("SoulmateList.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Soulmate(FileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 1771;
    }
}
