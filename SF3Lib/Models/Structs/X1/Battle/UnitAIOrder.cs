using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class UnitAIOrder : Struct {
        private readonly int _targetAddr;
        private readonly int _targetFlagsAddr;
        private readonly int _aggrAddr;

        public UnitAIOrder(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x03) {
            _targetAddr      = Address + 0x00;
            _targetFlagsAddr = Address + 0x01;
            _aggrAddr        = Address + 0x02;
        }

        [TableViewModelColumn(addressField: nameof(_targetAddr), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public byte Target {
            get => (byte) Data.GetUInt8(_targetAddr);
            set => Data.SetUInt8(_targetAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_targetFlagsAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public byte TargetFlags {
            get => (byte) Data.GetUInt8(_targetFlagsAddr);
            set => Data.SetUInt8(_targetFlagsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_aggrAddr), displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public byte Aggression {
            get => (byte) Data.GetUInt8(_aggrAddr);
            set => Data.SetUInt8(_aggrAddr, value);
        }

        public AIOrderType Type {
            get {
                var target = Target;
                if (target == 0)
                    return AIOrderType.Leader;
                else if (target == 1)
                    return AIOrderType.Closest;
                else if (target >= 0x32 && target < 0x51)
                    return AIOrderType.Location;
                else if (target >= 0x80 && target < 0xBF)
                    return AIOrderType.Unit;
                else if (target >= 0xC0 && target < 0xDF)
                    return AIOrderType.Path;
                else if (target == 0xFF)
                    return AIOrderType.NoOrder;
                else
                    return AIOrderType.Invalid;
            }
        }

        public bool Exists => Target != 0xFF;
    }
}
