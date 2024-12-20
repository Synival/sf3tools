using CommonLib.NamedValues;
using SF3.FileEditors;
using SF3.Models;
using SF3.Types;

namespace SF3.Tables {
    public class SpellIconTable : Table<SpellIcon> {
        public SpellIconTable(IByteEditor fileEditor, string resourceFile, int address, bool has16BitIconAddr, int realOffsetStart)
        : base(fileEditor, resourceFile, address) {
            Has16BitIconAddr = has16BitIconAddr;
            RealOffsetStart  = realOffsetStart;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpellIcon(FileEditor, id, name, address, Has16BitIconAddr, RealOffsetStart));

        public override int? MaxSize => 256;

        public bool Has16BitIconAddr { get; }
        public int RealOffsetStart { get; }
    }
}
