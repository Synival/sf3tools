using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD;
using SF3.Types;

namespace SF3.Models.Tables.MPD {
    public class MPDHeaderTable : FixedSizeTable<MPDHeaderModel> {
        protected MPDHeaderTable(IByteData data, string name, int address, ScenarioType scenario) : base(data, name, address, 1) {
            Scenario = scenario;
        }

        public static MPDHeaderTable Create(IByteData data, string name, int address, ScenarioType scenario) {
            var newTable = new MPDHeaderTable(data, name, address, scenario);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new MPDHeaderModel(Data, id, "Header", address, Scenario));

        public ScenarioType Scenario { get; }
    }
}
