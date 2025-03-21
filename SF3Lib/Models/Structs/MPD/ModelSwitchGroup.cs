using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD {
    public class ModelSwitchGroup : Struct {
        private readonly int _triggerFlagAddr;
        private readonly int _enabledModelsOffsetAddr;
        private readonly int _disabledModelsOffsetAddr;
        private readonly int _triggerStateAddr;

        public ModelSwitchGroup(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10) {
            _triggerFlagAddr          = Address + 0x00; // 4 bytes
            _enabledModelsOffsetAddr  = Address + 0x04; // 4 bytes
            _disabledModelsOffsetAddr = Address + 0x08; // 4 bytes
            _triggerStateAddr         = Address + 0x0C; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, displayFormat: "X3", minWidth: 200)]
        [NameGetter(NamedValueType.GameFlag)]
        public int TriggerFlag {
            get => Data.GetDouble(_triggerFlagAddr);
            set => Data.SetDouble(_triggerFlagAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1, isPointer: true)]
        public uint EnabledModelsOffset {
            get => (uint) Data.GetDouble(_enabledModelsOffsetAddr);
            set => Data.SetDouble(_enabledModelsOffsetAddr, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, isPointer: true)]
        public uint DisabledModelsOffset {
            get => (uint) Data.GetDouble(_disabledModelsOffsetAddr);
            set => Data.SetDouble(_disabledModelsOffsetAddr, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 3, displayFormat: "X2")]
        public uint TriggerState {
            get => (uint) Data.GetDouble(_triggerStateAddr);
            set => Data.SetDouble(_triggerStateAddr, (int) value);
        }
    }
}
