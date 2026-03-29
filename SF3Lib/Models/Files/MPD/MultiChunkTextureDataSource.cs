using System;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using SF3.ByteData;

namespace SF3.Models.Files.MPD {
    public class MultiChunkTextureDataSource : ITextureDataSource {
        public const int c_width = 512;

        public MultiChunkTextureDataSource(IByteData[] datas, bool isTiled) {
            Datas   = datas;
            IsTiled = isTiled;

            if (datas != null) {
                for (int i = 0; i < datas.Length; i++)
                    if (datas[i] != null && datas[i].Length % c_width != 0)
                        throw new ArgumentException($"{nameof(datas)}[{i}] height is not divisible by 512");
            }

            Height = (datas == null) ? 0 : datas.Select(x => (x?.Length ?? 0) / c_width).Sum();
            StoredImageDataSize = c_width * Height;
        }

        public byte[,] FetchImageData8Bit(ITextureData tex) {
            if (Datas == null)
                return null;

            var dataBytes = Datas.Where(x => x != null).Select(x => x.GetDataCopyOrReference()).ToArray();
            var fullDataHeight = dataBytes.Select(x => x.Length / c_width).Sum();
            var fullDataBytes = new byte[c_width * fullDataHeight];

            int pos = 0;
            foreach (var data in dataBytes) {
                data.CopyTo(fullDataBytes, pos);
                pos += data.Length;
            }

            return IsTiled ? fullDataBytes.ToTiles(c_width, fullDataHeight, 8, 8) : fullDataBytes.To2DArrayColumnMajor(c_width, fullDataHeight);
        }

        public ushort[,] FetchImageData16Bit(ITextureData tex) => throw new NotImplementedException();

        public object ConvertImageDataToStorageData8Bit(ITextureData tex, byte[,] data, IPalette palette, out int? storageSize) {
            var newData = IsTiled ? data.FromTiles(8, 8) : data.To1DArrayTransposed();
            storageSize = newData.Length;
            return newData;
        }

        public object ConvertImageDataToStorageData16Bit(ITextureData tex, ushort[,] data, out int? storageSize) => throw new NotImplementedException();

        public void StoreImageData(ITextureData data, object storageData) {
            var toDatas = Datas?.Where(x => x != null)?.ToArray() ?? new IByteData[0];
            var fromData = (byte[]) storageData;
            int fromDataPos = 0;

            foreach (var toData in toDatas) {
                var newData = new byte[toData.Length];
                Array.Copy(fromData, fromDataPos, newData, 0, toData.Length);
                fromDataPos += toData.Length;
                toData.Data.SetDataTo(newData);
            }
        }

        public int Width => c_width;
        public int Height { get; }

        public IByteData[] Datas { get; }
        public bool IsTiled { get; }
        public int? StoredImageDataSize { get; private set; }
    }
}
