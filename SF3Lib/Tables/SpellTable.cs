using SF3.FileEditors;
using SF3.Models;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables {
    public class SpellTable : Table<Spell> {
        public override int? MaxSize => 78;

        public SpellTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFileForScenario(FileEditor.Scenario, "Spells.xml");
            Address = address;
        }

        public override string ResourceFile { get; }
        public override int Address { get; }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Spell(FileEditor, id, name, address));
    }
}
