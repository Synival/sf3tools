using SF3.RawEditors;
using SF3.Models.X1.Battle;

namespace SF3.TableModels.X1.Battle {
    public class CustomMovementTable : Table<CustomMovement> {
        public CustomMovementTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new CustomMovement(Editor, id, name, address));

        public override int? MaxSize => 130;
    }
}
