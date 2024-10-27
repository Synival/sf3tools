using SF3.FileEditors;
using SF3.Models.IconPointerEditor;
using static SF3.Utils.Resources;

namespace SF3.Tables.IconPointerEditor {
    public class ItemIconTable : Table<ItemIcon> {
        public override int? MaxSize => 300;

        public ItemIconTable(IIconPointerFileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
            _resourceFile = ResourceFileForScenario(_fileEditor.Scenario, "Items.xml");
        }

        private readonly string _resourceFile;
        private readonly IIconPointerFileEditor _fileEditor;

        public override string ResourceFile => _resourceFile;

        public override bool Load()
            => LoadFromResourceFile((value, name) => new ItemIcon(_fileEditor, value, name));
    }
}
