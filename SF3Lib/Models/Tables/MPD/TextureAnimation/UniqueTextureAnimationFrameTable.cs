using System;
using System.Collections.Generic;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.TextureAnimation;

namespace SF3.Models.Tables.MPD.TextureAnimation {
    public class UniqueTextureAnimationFrameTable : Table<UniqueTextureAnimationFrame> {
        protected UniqueTextureAnimationFrameTable(IByteData data, string name, int address, IMPD_File mpdFile)
        : base(data, name, address) {
            MPD_File = mpdFile;
        }

        public override int TerminatorSize => 0;
        public override bool IsContiguous => true;

        public IMPD_File MPD_File { get; }

        public static UniqueTextureAnimationFrameTable Create(IByteData data, string name, int address, IMPD_File mpdFile)
            => Create(() => new UniqueTextureAnimationFrameTable(data, name, address, mpdFile));

        public override bool Load() {
            var rowDict = new Dictionary<int, UniqueTextureAnimationFrame>();
            var rows = new List<UniqueTextureAnimationFrame>();

            try {
                var address = Address;
                var rawData = Data.GetDataCopyOrReference();

                for (var id = 0; id < 0x1000 && address < Data.Length; ++id) {
                    var decompressed = Compression.DecompressLZSS(rawData, address, maxOutput: null, out _, out _);

                    // TODO: get rid of all this big stupid guessing!
                    var size = decompressed.Length;
                    int width = 0, height = 0;
                    bool isIndexed = false;
                    if (size % 2 == 0) {
                        var size2 = size / 2;
                        var divisor = (int) Math.Sqrt(size2);
                        while (divisor > 1) {
                            var quotient = size2 / (double) divisor;
                            if (quotient == (int) quotient) {
                                width = divisor;
                                height = (int) quotient;
                                break;
                            }
                            divisor--;
                        }
                    }
                    else {
                        var divisor = (int) Math.Sqrt(size);
                        while (divisor > 1) {
                            var quotient = size / (double) divisor;
                            if (quotient == (int) quotient) {
                                width = divisor;
                                height = (int) quotient;
                                isIndexed = true;
                                break;
                            }
                            divisor--;
                        }
                    }

                    if (width == 0) {
                        if (size % 2 == 0) {
                            width = 1;
                            height = size / 2;
                        }
                        else {
                            width = 1;
                            height = size;
                            isIndexed = true;
                        }
                    }

                    var newModel = new UniqueTextureAnimationFrame(Data, id, $"TexAnimFrame_{id:D3}", address, width, height, isIndexed, MPD_File);

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
    }
}
