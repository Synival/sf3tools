using System;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
    public class Warp : Struct {
        public Warp(IByteData data, int id, string name, int address, Warp prevWarp, INameGetterContext nameGetterContext)
        : base(data, id, name, address, 0x04) {
            PrevWarp = prevWarp;
            NameGetterContext = nameGetterContext;

            Name = "Warp_" + MapName + "_" + (WarpID + 1).ToString("D2");
        }

        public Warp PrevWarp { get; }
        public INameGetterContext NameGetterContext { get; }

        public string MapName {
            get {
                var prevWarp = PrevWarp;
                return (prevWarp == null || WarpTrigger == 0)
                    ? NameGetterContext.GetName(null, null, LoadID, new object[] { NamedValueType.Load })
                    : prevWarp.MapName;
            }
        }

        public int WarpID {
            get {
                var prevWarp = PrevWarp;
                return (prevWarp == null || WarpTrigger == 0) ? 0 : prevWarp.WarpID + 1;
            }
        }

        private uint RawData {
            get => (uint) Data.GetDouble(Address);
            set => Data.SetDouble(Address, (int) value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public byte EntranceID {
            // First 5 bits (F800,0000)
            get => (byte) ((RawData & 0xF800_0000u) >> 27);
            set => RawData = (RawData & ~0xF800_0000u) | ((uint) (value << 27) & 0xF800_0000u);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X3")]
        [BulkCopy]
        public ushort Flag {
            // Next 12 bits (07FF,8000)
            get => (ushort) ((RawData & 0x07FF_8000u) >> 15);
            set => RawData = (RawData & ~0x07FF_8000u) | ((uint) (value << 15) & 0x07FF_8000u);
        }

        [TableViewModelColumn(displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public byte WarpTrigger {
            // Next 6 bits (0000,7E00)
            get => (byte) ((RawData & 0x0000_7E00u) >> 9);
            set => RawData = (RawData & ~0x0000_7E00u) | ((uint) (value << 9) & 0x0000_7E00u);
        }

        [TableViewModelColumn(displayOrder: 4, minWidth: 120)]
        [BulkCopy]
        [NameGetter(NamedValueType.Load)]
        public int LoadID {
            // Next 9 bits (0000,01FF)
            get => (int) (RawData & 0x0000_01FF);
            set => RawData = (uint) ((RawData & ~0x0000_01FFu) | (value & 0x0000_01FFu));
        }

        [TableViewModelColumn(displayOrder: 4, displayName: "MPD Tie-In", displayFormat: "X2")]
        public byte? MPDTieIn {
            get {
                var warpId = WarpTrigger;
                var loadId = LoadID;
                return (warpId >= 0x01 && warpId <= 0x0F && loadId != 0x1FF) ? (byte) (warpId + 0x10u) : (byte?) null;
            }
        }
    }
}
