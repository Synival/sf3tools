using SF3.FileEditors;
using SF3.Models.X1;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables.X1 {
    public class TileMovementTable : Table<TileMovement> {
        public TileMovementTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("MovementTypes.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new TileMovement(FileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 31;
    }
}
