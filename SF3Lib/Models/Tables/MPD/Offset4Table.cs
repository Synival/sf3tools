using SF3.Models.Structs.MPD;
using SF3.RawData;

namespace SF3.Models.Tables.MPD {
    public class Offset4Table : Table<Offset4Model> {
        protected Offset4Table(IByteData data, int address) : base(data, address) {
        }

        public static Offset4Table Create(IByteData data, int address) {
            var newTable = new Offset4Table(data, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load() {
            return LoadUntilMax(
                (id, address) => {
                    var atEnd = (uint) Data.GetDouble(address) == 0xFFFF_FFFF;
                    return new Offset4Model(Data, id, atEnd ? "--" : "Row " + id, address);
                },
                (currentRows, model) => model.Unknown1 != 0xFFFF_FFFF);
        }
    }
}
