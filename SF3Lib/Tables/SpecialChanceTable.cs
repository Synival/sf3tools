using SF3.FileEditors;
using SF3.Models;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables {
    public class SpecialChanceTable : Table<SpecialChance> {
        public SpecialChanceTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("SpecialChanceList.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpecialChance(FileEditor, id, name, address, Scenario < ScenarioType.Scenario3));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 1;
    }
}
