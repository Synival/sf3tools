using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using static CommonLib.Imaging.PixelConversion;

namespace SF3.Models.Structs.MPD {
    public class ColorModel : Struct {
        public ColorModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 2) {
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Color (ABGR1555)", displayOrder: 0, displayFormat: "X4")]
        public ushort ColorABGR1555 {
            get => (ushort) Data.GetWord(Address);
            set => Data.SetWord(Address, value);
        }

        [TableViewModelColumn(displayName: "HTML Color", displayOrder: 1, displayFormat: "X", minWidth: 80)]
        public string HtmlColor {
            get => ABGR1555toChannels((ushort) Data.GetWord(Address)).ToHtmlColor();
            set {
                if (!IsValidHtmlColor(value))
                    return;
                ColorABGR1555 = PixelChannels.FromHtmlColor(value, 0).ToABGR1555();
            }
        }
    }
}
