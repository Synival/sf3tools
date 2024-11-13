using SF3.FileEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class ColorTable : Table<Color> {
        public ColorTable(IByteEditor fileEditor, int address, int colors) : base(fileEditor, address) {
            MaxSize = colors;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new Color(FileEditor, id, "Color" + id, address));

        public override int? MaxSize { get; }
    }
}
