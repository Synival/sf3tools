using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.Shared {
    /// <summary>
    /// This is only found in RAM. They appear at a table here (about 0x20 entries or so):
    ///     Scn1:   0x06022980
    ///     Scn2v2: 0x060241C0
    ///     Scn3:   0x06024340
    /// </summary>
    public class UnknownUpdateFunction : Struct {
        private readonly int _functionAddr;
        private readonly int _unknown0x04Addr;
        private readonly int _unknown0x06Addr;
        private readonly int _unknown0x07Addr;

        public UnknownUpdateFunction(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x08) {
            _functionAddr    = Address + 0x00; // 4 bytes
            _unknown0x04Addr = Address + 0x04; // 2 bytes
            _unknown0x06Addr = Address + 0x06; // 1 byte
            _unknown0x07Addr = Address + 0x07; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_functionAddr), displayOrder: 0, isPointer: true)]
        public uint FunctionAddr {
            get => Data.GetUInt32(_functionAddr);
            set => Data.SetUInt32(_functionAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x04Addr), displayOrder: 1, displayName: nameof(Unknown0x04), displayFormat: "X4")]
        public short Unknown0x04 {
            get => Data.GetInt16(_unknown0x04Addr);
            set => Data.SetInt16(_unknown0x04Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x06Addr), displayOrder: 2, displayName: nameof(Unknown0x06) + " (bool? pre/post indicator?)", displayFormat: "X2")]
        public byte Unknown0x06 {
            get => Data.GetUInt8(_unknown0x06Addr);
            set => Data.SetUInt8(_unknown0x06Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x07Addr), displayOrder: 3, displayName: nameof(Unknown0x07) + " (seems unused, but probably not)", displayFormat: "X2")]
        public byte Unknown0x07 {
            get => Data.GetUInt8(_unknown0x07Addr);
            set => Data.SetUInt8(_unknown0x07Addr, value);
        }
    }
}
