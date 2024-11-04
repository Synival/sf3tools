using SF3.FileEditors;
using SF3.Models.X013;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables.X013 {
    public class CritrateTable : Table<Critrate> {
        public CritrateTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("CritrateList.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Critrate(FileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 3;
    }
}
