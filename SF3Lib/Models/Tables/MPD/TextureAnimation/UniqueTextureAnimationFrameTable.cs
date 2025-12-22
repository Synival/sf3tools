using System;
using System.Collections.Generic;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.TextureAnimation;

namespace SF3.Models.Tables.MPD.TextureAnimation {
    public readonly struct UniqueTextureAnimationFrameInfo {
        public UniqueTextureAnimationFrameInfo(int width, int height, bool isIndexed) {
            Width     = width;
            Height    = height;
            IsIndexed = isIndexed;
        }

        public readonly int Width;
        public readonly int Height;
        public readonly bool IsIndexed;
    }

    public class UniqueTextureAnimationFrameTable : Table<UniqueTextureAnimationFrame> {
        protected UniqueTextureAnimationFrameTable(IByteData data, string name, int address, Dictionary<int, UniqueTextureAnimationFrameInfo> infoByOffset, IMPD_File mpdFile)
        : base(data, name, address) {
            InfoByOffset = infoByOffset;
            MPD_File = mpdFile;
        }

        public override int TerminatorSize => 0;
        public override bool IsContiguous => true;

        public static UniqueTextureAnimationFrameTable Create(IByteData data, string name, int address, Dictionary<int, UniqueTextureAnimationFrameInfo> infoByOffset, IMPD_File mpdFile)
            => Create(() => new UniqueTextureAnimationFrameTable(data, name, address, infoByOffset, mpdFile));

        public override bool Load() {
            var rowDict = new Dictionary<int, UniqueTextureAnimationFrame>();
            var rows = new List<UniqueTextureAnimationFrame>();

            try {
                var address = Address;
                var rawData = Data.GetDataCopyOrReference();

                for (var id = 0; id < 0x1000 && address < Data.Length; ++id) {
                    var decompressed = Compression.DecompressLZSS(rawData, address, maxOutput: null, out _, out _);

                    var size = decompressed.Length;
                    int width = 0, height = 0;
                    bool isIndexed = false;
                    bool isKnown = false;

                    // Get the width/height/"is indexed" value, if known.
                    if (InfoByOffset.TryGetValue(address, out var infoOut)) {
                        (width, height, isIndexed) = (infoOut.Width, infoOut.Height, infoOut.IsIndexed);
                        isKnown = true;
                    }
                    else
                    // Otherwise, make a big, stupid guess.
                        (width, height, isIndexed) = GuessDimensions(size);

                    var newModel = new UniqueTextureAnimationFrame(Data, id, $"TexAnimFrame_{id:D3}", address, width, height, isIndexed, isKnown, MPD_File);

                    rowDict[id] = newModel;
                    rows.Add(newModel);

                    address += newModel.StoredImageDataSize;
                    if (address % 4 != 0)
                        address += 4 - (address % 4);
                }
            }
            catch {
                return false;
            }
            finally {
                _rows = rows.ToArray();
            }
            return true;
        }

        private (int Width, int Height, bool IsIndexed) GuessDimensions(int size) {
            if (size % 2 == 0) {
                var size2 = size / 2;
                var divisor = (int) Math.Sqrt(size2);
                while (divisor > 1) {
                    var quotient = size2 / (double) divisor;
                    if (quotient == (int) quotient)
                        return (divisor, (int) quotient, false);
                    divisor--;
                }
            }
            else {
                var divisor = (int) Math.Sqrt(size);
                while (divisor > 1) {
                    var quotient = size / (double) divisor;
                    if (quotient == (int) quotient)
                        return (divisor, (int) quotient, true);
                    divisor--;
                }
            }

            if (size % 2 == 0)
                return (1, size / 2, false);
            else
                return (1, size, true);
        }

        public Dictionary<int, UniqueTextureAnimationFrameInfo> InfoByOffset { get; }
        public IMPD_File MPD_File { get; }
    }
}
