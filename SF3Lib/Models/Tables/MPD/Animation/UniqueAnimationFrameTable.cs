using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Images;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Animation;

namespace SF3.Models.Tables.MPD.Animation {
    public readonly struct UniqueAnimationFrameInfo {
        public UniqueAnimationFrameInfo(int width, int height, bool isIndexed) {
            Width     = width;
            Height    = height;
            IsIndexed = isIndexed;
        }

        public readonly int Width;
        public readonly int Height;
        public readonly bool IsIndexed;
    }

    public class UniqueAnimationFrameTable : Table<UniqueAnimationFrame> {
        protected UniqueAnimationFrameTable(IByteData data, string name, int address, Dictionary<int, UniqueAnimationFrameInfo> infoByOffset, IMPD_File mpdFile)
        : base(data, name, address) {
            InfoByOffset = infoByOffset;
            MPD_File = mpdFile;
        }

        public override int TerminatorSize => 0;
        public override bool IsContiguous => true;

        public static UniqueAnimationFrameTable Create(IByteData data, string name, int address, Dictionary<int, UniqueAnimationFrameInfo> infoByOffset, IMPD_File mpdFile)
            => Create(() => new UniqueAnimationFrameTable(data, name, address, infoByOffset, mpdFile));

        public override bool Load() {
            var rowDict = new Dictionary<int, UniqueAnimationFrame>();
            var rows = new List<UniqueAnimationFrame>();

            try {
                var address = Address;
                var rawData = Data.GetDataCopyOrReference();

                for (var id = 0; id < 0x1000 && address < Data.Length; ++id) {
                    var decompressed = Compression.DecompressLZSS(rawData, address, maxOutput: null, out _, out _);

                    var size = decompressed.Length;
                    int width = 0, height = 0;
                    var isIndexed = false;
                    var isKnown = false;

                    // Get the width/height/"is indexed" value, if known.
                    if (InfoByOffset.TryGetValue(address, out var infoOut)) {
                        (width, height, isIndexed) = (infoOut.Width, infoOut.Height, infoOut.IsIndexed);
                        isKnown = true;
                    }
                    else {
                        // Otherwise, make a big, stupid guess.
                        int bytesPerPixel;
                        (width, height, bytesPerPixel) = MathHelpers.GuessImageDimensions(size, canBe8Bit: true, canBe16Bit: true);
                        isIndexed = bytesPerPixel == 1;
                    }

                    var newModel = new UniqueAnimationFrame(Data, id, $"TexAnimFrame_{id:D3}", address, width, height, isIndexed, isKnown, MPD_File);

                    rowDict[id] = newModel;
                    rows.Add(newModel);

                    address += newModel.StoredImageDataSize;
                    if (address % 4 != 0)
                        address += 4 - address % 4;
                }
            }
            catch {
                return false;
            }
            finally {
                _rows = rows.ToArray();
            }

            if (_rows != null)
                FrameByOffset = _rows.ToDictionary(x => x.ImageDataOffset, x => x);

            return true;
        }

        public ITexture AtOffset(int offset)
            => (FrameByOffset?.TryGetValue(offset, out var frame) == true) ? frame : null;

        public Dictionary<int, UniqueAnimationFrameInfo> InfoByOffset { get; }
        public Dictionary<int, UniqueAnimationFrame> FrameByOffset { get; private set; }
        public IMPD_File MPD_File { get; }
    }
}
