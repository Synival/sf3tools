using SF3.FileEditors;
using SF3.Models.X1;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables.X1 {
    public class AITable : Table<AI> {
        public AITable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("X1AI.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new AI(FileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 130;
    }
}
