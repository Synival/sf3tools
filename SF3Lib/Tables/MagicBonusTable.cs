using SF3.FileEditors;
using SF3.Models;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables {
    public class MagicBonusTable : Table<MagicBonus> {
        public MagicBonusTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFileForScenario(fileEditor.Scenario, "MagicBonus.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new MagicBonus(FileEditor, id, name, address, Scenario == ScenarioType.Scenario1));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 256;
    }
}
