using SF3.FileEditors;
using SF3.Models.X002;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables.X002 {
    public class AttackResistTable : Table<AttackResist> {
        public override int? MaxSize => 2;

        public AttackResistTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("AttackResistList.xml");
            Address = address;
        }

        public override string ResourceFile { get; }
        public override int Address { get; }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new AttackResist(FileEditor, id, name));
    }
}
