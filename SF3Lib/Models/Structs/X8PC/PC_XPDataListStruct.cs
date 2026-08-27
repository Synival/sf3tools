using System.Linq;
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

        public ISGL_Model[] GetAllModels() {
            var offset = XPDataListOffset;
            return PolyChar.XPDataTables[ID].ToArray();
        }

        public PolyChar PolyChar { get; }
    }
}
