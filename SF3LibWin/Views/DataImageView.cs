using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.Win.Types;

namespace SF3.Win.Views {
    public class DataImageView : ImageView {
        public DataImageView(string name, IByteArray data, Palette palette, DataImageViewMode viewMode) : this(name, [data], [palette], viewMode) { }

        public DataImageView(string name, IByteArray data, Palette[] palettes, DataImageViewMode viewMode) : this(name, [data], palettes, viewMode) { }

        public DataImageView(string name, IByteArray[] datas, Palette palette, DataImageViewMode viewMode) : this(name, datas, [palette], viewMode) { }

        public DataImageView(string name, IByteArray[] datas, Palette[] palettes, DataImageViewMode viewMode = DataImageViewMode.ColumnMajor) : base(name, 2) {
            Datas    = datas;
            Palettes = palettes;
            ViewMode = viewMode;
        }

        public override Control Create() {
            var rval = base.Create();
            if (rval == null)
                return rval;

            UpdateImage();
            return rval;
        }

        public override void RefreshContent() {
            base.RefreshContent();
            if (IsCreated)
                UpdateImage();
        }

        public void UpdateImage() {
            const int width = 512;
            var heights = Datas.Select(x => x.Length / width).ToArray();

            var bitmapHeight = heights.Sum() * Palettes.Length;
            var bitmapData = new byte[width * bitmapHeight * 4];

            var pos = 0;
            foreach (var palette in Palettes) {
                for (int i = 0; i < heights.Length; i++) {
                    var height = heights[i];
                    var data = Datas[i];

                    byte[,] imageData;
                    var dataCopy = data.GetDataCopyAt(0, width * height);
                    switch (ViewMode) {
                        case DataImageViewMode.RowMajor:
                            imageData = dataCopy.To2DArray(width, height);
                            break;

                        case DataImageViewMode.ColumnMajor:
                            imageData = dataCopy.To2DArrayColumnMajor(width, height);
                            break;

                        case DataImageViewMode.Tiles8x8:
                            imageData = dataCopy.ToTiles(width, height, 8, 8);
                            break;

                        default:
                            throw new InvalidOperationException($"Unhandled {nameof(ViewMode)}: {ViewMode}");
                    }

                    var paletteBitmapData = BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(imageData, palette, false);
                    paletteBitmapData.CopyTo(bitmapData, pos);
                    pos += width * height * 4;
                }
            }

            var bitmap = new Bitmap(width, bitmapHeight, PixelFormat.Format32bppArgb);
            unsafe {
                var bitmapLock = bitmap.LockBits(new Rectangle(0, 0, width, bitmapHeight), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                Marshal.Copy(bitmapData, 0, bitmapLock.Scan0, bitmapData.Length);
                bitmap.UnlockBits(bitmapLock);
            }

            Image = bitmap;
        }

        public IByteArray[] Datas { get; }
        public Palette[] Palettes { get; }
        public DataImageViewMode ViewMode { get; }
    }
}
