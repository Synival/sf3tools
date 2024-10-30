using SF3.FileEditors;
using SF3.Models;
using static SF3.Utils.Resources;

namespace SF3.Tables {
    public class StatsTable : Table<Stats> {
        public StatsTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFileForScenario(Scenario, "ClassList.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Stats(FileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 300;
    }
}
