using SF3.FileEditors;
using SF3.Models.X013;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables.X013 {
    public class SpecialTable : Table<Special> {
        public SpecialTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFileForScenario(FileEditor.Scenario, "Specials.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Special(FileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 256;
    }
}
