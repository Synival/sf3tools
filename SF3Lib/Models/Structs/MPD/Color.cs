using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.MPD {
    public class Color : Struct {
        public Color(IRawData editor, int id, string name, int address)
        : base(editor, id, name, address, 2) {
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Color (ABGR1555)", displayOrder: 0, displayFormat: "X4")]
        public int ColorABGR1555 {
            get => Data.GetWord(Address);
            set => Data.SetWord(Address, value);
        }

        [TableViewModelColumn(displayName: "HTML Color", displayOrder: 1, displayFormat: "X", minWidth: 80)]
        public string HtmlColor {
            get {
                // TODO: behavior for the 0x8000 bit
                var value = (ushort) Data.GetWord(Address);
                var r = value >>  0 & 0x1F;
                var g = value >>  5 & 0x1F;
                var b = value >> 10 & 0x1F;

                return ((value & 0x8000) != 0 ? "1|#" : "0|#") +
                    (r * 255 / 31).ToString("X2") +
                    (g * 255 / 31).ToString("X2") +
                    (b * 255 / 31).ToString("X2");
            }
        }
    }
}
