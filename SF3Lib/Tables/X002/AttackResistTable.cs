using SF3.StreamEditors;
using SF3.Models.X002;

namespace SF3.Tables.X002 {
    public class AttackResistTable : Table<AttackResist> {
        public AttackResistTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new AttackResist(FileEditor, id, name, address));

        public override int? MaxSize => 2;
    }
}
