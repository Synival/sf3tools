using System.Collections;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.Models.Structs.X8PC {
    public class PC_XPDataListStruct : Struct, ISGL_ModelCollection {
        private readonly int _xpdataListOffsetAddr;

        public PC_XPDataListStruct(IByteData data, int id, string name, int address, PolyChar polyChar)
        : base(data, id, name, address, 0x04) {
            PolyChar = polyChar;

            _xpdataListOffsetAddr = Address + 0x00; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_xpdataListOffsetAddr), displayOrder: 0, isPointer: true)]
        public int XPDataListOffset {
            get => Data.GetInt32(_xpdataListOffsetAddr);
            set => Data.SetInt32(_xpdataListOffsetAddr, value);
        }

        public ISGL_Model GetModel(int id, int lod) {
            var offset = XPDataListOffset;
            return PolyChar.XPDataTables[ID][id];
        }

        public IEnumerator<ISGL_Model> GetEnumerator() => PolyChar.XPDataTables[ID].GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public PolyChar PolyChar { get; }
    }
}
