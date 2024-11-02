using SF3.FileEditors;
using SF3.Models;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables {
    public class LoadedOverrideTable : Table<LoadedOverride> {
        public LoadedOverrideTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFileForScenario(FileEditor.Scenario, "LoadedOverrideList.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new LoadedOverride(FileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 300;
    }
}
