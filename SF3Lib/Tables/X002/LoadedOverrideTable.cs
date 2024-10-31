using SF3.FileEditors;
using SF3.Models.X002;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables.X002 {
    public class LoadedOverrideTable : Table<LoadedOverride> {
        public override int? MaxSize => 300;

        public LoadedOverrideTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFileForScenario(FileEditor.Scenario, "LoadedOverrideList.xml");
            Address = address;
        }

        public override string ResourceFile { get; }
        public override int Address { get; }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new LoadedOverride(FileEditor, id, name));
    }
}
