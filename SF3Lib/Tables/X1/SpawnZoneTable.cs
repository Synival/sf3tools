using SF3.FileEditors;
using SF3.Models.X1;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables.X1 {
    public class SpawnZoneTable : Table<SpawnZone> {
        public SpawnZoneTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("UnknownAIList.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpawnZone(FileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 30;
    }
}
