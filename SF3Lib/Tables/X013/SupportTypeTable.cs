using SF3.FileEditors;
using SF3.Models.X013;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables.X013 {
    public class SupportTypeTable : Table<SupportType> {
        public SupportTypeTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFileForScenario(fileEditor.Scenario, "Characters.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SupportType(FileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 120;
    }
}
