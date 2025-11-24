using System;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.SurfaceModel {
    public class VertexHeightBlock : Struct {
        private const int c_meshWidth  = 5;
        private const int c_meshHeight = 5;
        private const int c_meshCount  = c_meshWidth * c_meshHeight;

        private readonly int[,] normalAddresses = new int[5, 5];

        public VertexHeightBlock(IByteData data, int id, string name, int address, int blockX, int blockY)
        : base(data, id, name, address, c_meshCount) {
            BlockX = blockX;
            BlockY = blockY;

            var pos = Address;
            for (var y = 0; y < c_meshHeight; y++)
                for (var x = 0; x < c_meshWidth; x++)
                    normalAddresses[x, y] = pos++;
        }

        public byte this[int x, int y] {
            get => (byte) Data.GetByte(normalAddresses[x, y]);
            set => Data.SetByte(normalAddresses[x, y], value);
        }

        public float GetHeight(int x, int y)
            => this[x, y] / 16.0f;
        public void SetHeight(int x, int y, float value)
            => this[x, y] = (byte) Math.Max(0, Math.Min(255, Math.Round(value * 16.0f)));

        public int BlockX { get; }
        public int BlockY { get; }
        public int BlockNum => ID;
    }
}
