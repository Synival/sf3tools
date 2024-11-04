using SF3.FileEditors;
using SF3.Models.X013;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables.X013 {
    public class MagicBonusTable : Table<MagicBonus> {
        public MagicBonusTable(IX013_FileEditor fileEditor, int address) : base(fileEditor) {
            _fileEditor = fileEditor;
            ResourceFile = ResourceFileForScenario(_fileEditor.Scenario, "MagicBonus.xml");
            Address = address;
        }

        private readonly IX013_FileEditor _fileEditor;

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new MagicBonus(_fileEditor, id, name, address, Scenario == ScenarioType.Scenario1));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 256;
    }
}
