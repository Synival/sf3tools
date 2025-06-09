using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using static CommonLib.Imaging.PixelConversion;

namespace SF3.Models.Structs.MPD {
    public class ColorModel : Struct {
        private readonly int _colorABGR1555Addr;

        public ColorModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 2) {
            _colorABGR1555Addr = Address + 0x00; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_colorABGR1555Addr), displayName: "Color (ABGR1555)", displayOrder: 0, displayFormat: "X4")]
        public ushort ColorABGR1555 {
            get => (ushort) Data.GetWord(_colorABGR1555Addr);
            set => Data.SetWord(_colorABGR1555Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_colorABGR1555Addr), displayName: "HTML Color", displayOrder: 1, displayFormat: "X", minWidth: 80)]
        public string HtmlColor {
            get => ABGR1555toChannels(ColorABGR1555).ToHtmlColor();
            set {
                if (!IsValidHtmlColor(value))
                    return;
                ColorABGR1555 = PixelChannels.FromHtmlColor(value, 0).ToABGR1555();
            }
        }
    }
}
