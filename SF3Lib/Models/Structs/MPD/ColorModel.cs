using System;
using CommonLib.Attributes;
using CommonLib.Utils;
using SF3.Models.Structs;
using SF3.RawData;
using static CommonLib.Utils.PixelConversion;

namespace SF3.Models.Structs.MPD {
    public class ColorModel : Struct {
        public ColorModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 2) {
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Color (ABGR1555)", displayOrder: 0, displayFormat: "X4")]
        public int ColorABGR1555 {
            get => Data.GetWord(Address);
            set => Data.SetWord(Address, value);
        }

        [TableViewModelColumn(displayName: "HTML Color", displayOrder: 1, displayFormat: "X", minWidth: 80)]
        public string HtmlColor {
            get => ABGR1555toChannels((ushort) Data.GetWord(Address)).ToHtmlColor();
            set {
                if (!IsValidHtmlColor(value))
                    return;
                ColorABGR1555 = PixelChannels.FromHtmlColor(value).ToABGR1555();
            }
        }
    }
}
