using System;
using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.SurfaceModel {
    public class VertexHeightBlock : Struct {
        private const int c_meshWidth  = 5;
        private const int c_meshHeight = 5;
        private const int c_meshCount  = c_meshWidth * c_meshHeight;

        private readonly int[,] heightAddresses = new int[5, 5];

        public VertexHeightBlock(IByteData data, int id, string name, int address, int blockX, int blockY)
        : base(data, id, name, address, c_meshCount) {
            BlockX = blockX;
            BlockY = blockY;

            var pos = Address;
            for (var y = 0; y < c_meshHeight; y++)
                for (var x = 0; x < c_meshWidth; x++)
                    heightAddresses[x, y] = pos++;
        }

        public byte this[int x, int y] {
            get => Data.GetUInt8(heightAddresses[x, y]);
            set => Data.SetUInt8(heightAddresses[x, y], value);
        }

        public byte GetHeight(int x, int y)
            => this[x, y];
        public void SetHeight(int x, int y, byte value)
            => this[x, y] = value;

        public int BlockNum => ID;

        [TableViewModelColumn(displayOrder: 0, isReadOnly: true, displayFormat: "D2")]
        public int BlockX { get; }

        [TableViewModelColumn(displayOrder: 1, isReadOnly: true, displayFormat: "D2")]
        public int BlockY { get; }

        // TODO: Let's make this actually editable I guess?
        [TableViewModelColumn(displayOrder: 2, isReadOnly: true, displayName: "Heights", minWidth: 500)]
        public string HeightsStr {
            get {
                var output = "";
                for (int y = 0; y < 5; y++)
                    for (int x = 0; x < 5; x++)
                        output += this[x, y] + " ";
                return output;
            }
        }
    }
}
