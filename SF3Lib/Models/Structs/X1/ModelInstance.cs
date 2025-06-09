using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.Models.Structs.X1 {
    public class ModelInstance : Struct {
        private readonly int _modelIdAddr;
        private readonly int _matrixBasisAddr;
        private readonly int _typeAddr;
        private readonly int _scriptAddr;

        public ModelInstance(IByteData data, int id, string name, int address, Dictionary<uint, ActorScript> actorScripts)
        : base(data, id, name, address, 0x08) {
            ActorScripts = actorScripts;

            _modelIdAddr     = Address + 0x00; // 2 bytes
            _matrixBasisAddr = Address + 0x02; // 1 bytes
            _typeAddr        = Address + 0x03; // 1 bytes
            _scriptAddr      = Address + 0x04; // 4 bytes
        }

        public Dictionary<uint, ActorScript> ActorScripts { get; set; }

        [TableViewModelColumn(addressField: nameof(_modelIdAddr), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public short ModelID {
            get => (short) Data.GetWord(_modelIdAddr);
            set => Data.SetWord(_modelIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_matrixBasisAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public byte MatrixBasis {
            get => (byte) Data.GetByte(_matrixBasisAddr);
            set => Data.SetByte(_matrixBasisAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_typeAddr), displayOrder: 2, minWidth: 100, displayFormat: "X2")]
        [NameGetter(NamedValueType.ModelInstanceType)]
        [BulkCopy]
        public int Type {
            get => Data.GetByte(_typeAddr);
            set => Data.SetByte(_typeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_scriptAddr), displayOrder: 3, isPointer: true, minWidth: 300)]
        [NameGetter(NamedValueType.ActorScript, nameof(ActorScripts))]
        [BulkCopy]
        public uint ScriptAddr {
            get => (uint) Data.GetDouble(_scriptAddr);
            set => Data.SetDouble(_scriptAddr, (int) value);
        }
    }
}
