using SF3.FileEditors;
using SF3.Models.X013;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables.X013 {
    public class SpecialTable : Table<Special> {
        public SpecialTable(IX013_FileEditor fileEditor, int address) : base(fileEditor) {
            _fileEditor = fileEditor;
            ResourceFile = ResourceFileForScenario(_fileEditor.Scenario, "Specials.xml");
            Address = address;
        }

        private readonly IX013_FileEditor _fileEditor;

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Special(_fileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 256;
    }
}
