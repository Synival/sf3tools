using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class ModelsHeader : Struct {
        private readonly int _unknownOffset1Address;
        private readonly int _unknownOffset2Address;
        private readonly int _numModelsAddress;

        public ModelsHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x0C) {
            _unknownOffset1Address = Address + 0x00; // 4 bytes
            _unknownOffset2Address = Address + 0x04; // 4 bytes
            _numModelsAddress      = Address + 0x08; // 2 bytes
            // (2 bytes of padding)
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, isPointer: true)]
        public int UnknownOffset1 {
            get => Data.GetDouble(_unknownOffset1Address);
            set => Data.SetDouble(_unknownOffset1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1, isPointer: true)]
        public int UnknownOffset2 {
            get => Data.GetDouble(_unknownOffset2Address);
            set => Data.SetDouble(_unknownOffset2Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2)]
        public int NumModels {
            get => Data.GetWord(_numModelsAddress);
            set => Data.SetWord(_numModelsAddress, value);
        }
    }
}
