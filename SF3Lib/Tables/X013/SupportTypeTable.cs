using SF3.FileEditors;
using SF3.Models.X013;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables.X013 {
    public class SupportTypeTable : Table<SupportType> {
        public SupportTypeTable(IX013_FileEditor fileEditor, int address) : base(fileEditor) {
            _fileEditor = fileEditor;
            ResourceFile = ResourceFileForScenario(_fileEditor.Scenario, "Characters.xml");
            Address = address;
        }

        private readonly IX013_FileEditor _fileEditor;

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SupportType(_fileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 120;
    }
}
