using System;
using CommonLib.SGL;
using SF3.RawData;

namespace SF3.Models.Structs.MPD {
    public class TileSurfaceVertexNormalMesh : Struct {
        private const int c_meshWidth  = 5;
        private const int c_meshHeight = 5;
        private const int c_meshCount  = c_meshWidth * c_meshHeight;

        private readonly int[,] normalAddresses = new int[5, 5];

        public TileSurfaceVertexNormalMesh(IByteData data, int id, string name, int address, int blockX, int blockY)
        : base(data, id, name, address, 6 * c_meshCount) {
            BlockX = blockX;
            BlockY = blockY;

            int pos = Address;
            for (var y = 0; y < c_meshHeight; y++) {
                for (var x = 0; x < c_meshWidth; x++) {
                    normalAddresses[x, y] = pos;
                    pos += 6;
                }
            }
        }

        public VECTOR this[int x, int y] {
            get {
                return new VECTOR(
                    new FIXED(Data.GetCompressedFIXED(normalAddresses[x, y] + 0)),
                    new FIXED(Data.GetCompressedFIXED(normalAddresses[x, y] + 2)),
                    new FIXED(Data.GetCompressedFIXED(normalAddresses[x, y] + 4))
                );
            }
            set {
                var abnormalAsCompressedFixed = value.PackageAbnormalForMPDFile();
                Data.SetCompressedFIXED(normalAddresses[x, y] + 0, abnormalAsCompressedFixed[0]);
                Data.SetCompressedFIXED(normalAddresses[x, y] + 2, abnormalAsCompressedFixed[1]);
                Data.SetCompressedFIXED(normalAddresses[x, y] + 4, abnormalAsCompressedFixed[2]);
            }
        }

        public int BlockX { get; }
        public int BlockY { get; }
        public int BlockNum { get; }
    }
}
