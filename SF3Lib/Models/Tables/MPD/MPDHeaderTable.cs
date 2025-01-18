using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD;
using SF3.Types;

namespace SF3.Models.Tables.MPD {
    public class MPDHeaderTable : Table<MPDHeaderModel> {
        protected MPDHeaderTable(IByteData data, int address, ScenarioType scenario) : base(data, address) {
            Scenario = scenario;
        }

        public static MPDHeaderTable Create(IByteData data, int address, ScenarioType scenario) {
            var newTable = new MPDHeaderTable(data, address, scenario);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new MPDHeaderModel(Data, id, "Header", address, Scenario));

        public override int? MaxSize => 1;
        public ScenarioType Scenario { get; }
    }
}
