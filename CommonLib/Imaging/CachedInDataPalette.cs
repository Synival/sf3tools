using System;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Utils;
using static CommonLib.Imaging.PixelConversion;

namespace CommonLib.Imaging {
    /// <summary>
    /// Color palette stored in data that can be updated. Reading from it reads from a cache, which can be updated via
    /// FetchColors(). Getting all colors from the 'Colors' property gets the reference for the cache, which does *not*
    /// update the data source when updated.
    /// </summary>
    public class CachedInDataPalette : IPalette {
        public CachedInDataPalette(IByteArray data, int paletteOffset, int count) {
            Data          = data;
            PaletteOffset = paletteOffset;
            ColorCount    = count;
            _colors       = new PixelChannels[count];
        }

        public void Invalidate() {
            _needsFetch = true;
        }

        public PixelChannels this[int index] {
            get {
                if (_needsFetch)
                    FetchColors();
                return _colors[index];
            }
            set {
                if (_needsFetch)
                    FetchColors();
                if (index < 0 || index >= ColorCount)
                    throw new IndexOutOfRangeException();

                _colors[index] = value;
                Data.SetDataAtTo(PaletteOffset + index * 2, 2, value.ToABGR1555().ToBytes());
            }
        }

        public int ColorCount { get; }

        private PixelChannels[] _colors;
        public PixelChannels[] Colors {
            get {
                if (_needsFetch)
                    FetchColors();
                return _colors;
            }
        }

        public void Replace(PixelChannels[] colors) {
            if (colors == null)
                throw new ArgumentNullException(nameof(colors));
            if (colors.Length != ColorCount)
                throw new ArgumentException($"'{nameof(colors)}' should have {ColorCount} colors, has {colors.Length}");

            _needsFetch = false;
            for (int i = 0; i < ColorCount; i++)
                _colors[i] = colors[i];

            var newColors = Colors.Select(x => x.ToABGR1555()).ToArray().ToBytes();
            Data.SetDataAtTo(PaletteOffset, newColors.Length, newColors);

        }

        private void FetchColors() {
            _needsFetch = false;
            var colors = Data.GetDataCopyAt(PaletteOffset, ColorCount * 2).ToUShorts();
            for (int i = 0; i < ColorCount; i++)
                _colors[i] = ABGR1555toChannels(colors[i]);
        }

        public IByteArray Data { get; }
        public int PaletteOffset { get; }

        private bool _needsFetch = true;
    }
}
