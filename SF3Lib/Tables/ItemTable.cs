using SF3.FileEditors;
using SF3.Models;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables {
    public class ItemTable : Table<Item> {
        public override int? MaxSize => 300;

        public ItemTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFileForScenario(FileEditor.Scenario, "Items.xml");
            Address = address;
        }

        public override string ResourceFile { get; }
        public override int Address { get; }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Item(FileEditor, id, name, address));
    }
}
