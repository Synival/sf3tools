using SF3.FileEditors;
using SF3.Models.X013;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables.X013 {
    public class SpecialChanceTable : Table<SpecialChance> {
        public SpecialChanceTable(IX013_FileEditor fileEditor, int address) : base(fileEditor) {
            _fileEditor = fileEditor;
            ResourceFile = ResourceFile("SpecialChanceList.xml");
            Address = address;
        }

        private readonly IX013_FileEditor _fileEditor;

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpecialChance(_fileEditor, id, name, address, Scenario < ScenarioType.Scenario3));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 1;
    }
}
