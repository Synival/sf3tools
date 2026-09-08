using SF3.ByteData;
using SF3.Models.Structs.X8PC;
using SF3.Types;

namespace SF3.Models.Tables.X8PC {
    public class PolyCharTable : AddressedTable<PolyChar> {
        protected PolyCharTable(IByteData data, string name, int[] addresses, ScenarioType scenario)
        : base(data, name, addresses) {
            Scenario = scenario;
        }

        public static PolyCharTable Create(IByteData data, string name, int[] addresses, ScenarioType scenario)
            => Create(() => new PolyCharTable(data, name, addresses, scenario));

        public override bool Load()
            => Load((id, address) => new PolyChar(Data, id, $"PolyChar{id + 1:D2} (@0x{address:X5})", address, Scenario));

        public ScenarioType Scenario { get; }
    }
}
