using System;
using SF3.ByteData;
using SF3.Models.Structs.X1;

namespace SF3.Models.Tables.X1 {
    public class MapUpdateFuncTable : TerminatedTable<MapUpdateFunc> {
        protected MapUpdateFuncTable(IByteData data, string name, int address, bool addEndModel = true)
        : base(data, name, address, 4, 0x20) {
            AddEndModel = addEndModel;
        }

        public static MapUpdateFuncTable Create(IByteData data, string name, int address, bool addEndModel = true) {
            var newTable = new MapUpdateFuncTable(data, name, address, addEndModel: addEndModel);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new MapUpdateFunc(Data, id, $"{nameof(MapUpdateFunc)}_{id:D2}", address),
                (rows, newModel) => newModel.UpdateSlot != 0xFFFFFFFF,
                addEndModel: AddEndModel);

        public bool AddEndModel { get; }
    }
}
