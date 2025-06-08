using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class BattleSceneFileIdSc1 : Struct {
        public BattleSceneFileIdSc1(IByteData data, int id, string name, int address, int? globalId)
        : base(data, id, name, address, 0x04) {
            GlobalId = globalId;
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0, displayFormat: "X2")]
        public int? GlobalId { get; }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndexWithFFFFFFFF)]
        [TableViewModelColumn(addressField: nameof(Address), displayOrder: 1, displayFormat: "X3", minWidth: 120)]
        public int FileId {
            get => Data.GetDouble(Address);
            set => Data.SetDouble(Address, value);
        }
    }
}
