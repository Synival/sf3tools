using SF3.FileEditors;
using SF3.Models.X1;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables.X1 {
    public class SlotTable : Table<Slot> {
        public SlotTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile(Scenario == ScenarioType.Scenario1 ? "X1List.xml" : "X1OtherList.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Slot(FileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 256;

    }
}
