using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD;
using SF3.Types;

namespace SF3.Models.Tables.MPD {
    public class LightAdjustmentTable : FixedSizeTable<LightAdjustmentModel> {
        protected LightAdjustmentTable(IByteData data, string name, int address, ScenarioType scenario) : base(data, name, address, 1) {
            Scenario = scenario;
        }

        public static LightAdjustmentTable Create(IByteData data, string name, int address, ScenarioType scenario) {
            var newTable = new LightAdjustmentTable(data, name, address, scenario);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new LightAdjustmentModel(Data, id, "Gradient", address, Scenario));

        public ScenarioType Scenario { get; }
    }
}
