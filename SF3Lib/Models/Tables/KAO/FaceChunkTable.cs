using System;
using System.Collections.Generic;
using CommonLib.Arrays;
using CommonLib.Logging;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Models.Structs.KAO;

namespace SF3.Models.Tables.KAO {
    public class FaceChunkTable : Table<FaceChunk> {
        protected FaceChunkTable(IByteData data, string name, int address)
        : base(data, name, address) {
        }

        public static FaceChunkTable Create(IByteData data, string name, int address)
            => Create(() => new FaceChunkTable(data, name, address));

        public override bool Load() {
            var headers = new List<FaceChunk>();

            const int headerAndPaletteSize = 0x222;
            int searchUntil = Data.Length - headerAndPaletteSize;

            int address = 0;
            int currentId = 0;
            int lastMaxSize = 0;
            FaceChunk lastFace = null;

            while (address < searchUntil) {
                FaceChunk newFace = null;
                try {
                    var width  = Data.GetUInt16(address + 0x02);
                    var height = Data.GetUInt16(address + 0x04);
                    if (width < 0x10 || width > 0x200 || height < 0x10 || height > 0x200) {
                        address += 0x800;
                        continue;
                    }

                    var decompressedData = Compression.DecompressLZSS(Data.GetDataCopyOrReference(), address, null, out var bytesRead, out var endDataFound);
                    if (!endDataFound) {
                        address += 0x800;
                        continue;
                    }

                    var compressedData = new CompressedData(new ByteArray(Data.GetDataCopyAt(address, bytesRead)));
                    newFace = new FaceChunk(compressedData.DecompressedData, currentId, $"{nameof(FaceChunk)}_{currentId:D2}", 0, address, compressedData);
                }
                catch (Exception e) {
                    Logger.LogException(e);
                    address += 0x800;
                    continue;
                }

                headers.Add(newFace);

                if (lastFace != null)
                    lastFace.MaxCompressedSize = lastMaxSize;
                lastMaxSize = ((newFace.CompressedSize + 0x7FF) / 0x800) * 0x800;

                address += Math.Max(0x800, lastMaxSize);
                currentId++;
                lastFace = newFace;
            }

            _rows = headers.ToArray();
            return true;
        }

        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;
    }
}
