using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.SurfaceModel {
    public class VertexNormalBlock : Struct {
        private const int c_meshWidth  = 5;
        private const int c_meshHeight = 5;
        private const int c_meshCount  = c_meshWidth * c_meshHeight;

        private readonly int[,] normalAddresses = new int[5, 5];

        public VertexNormalBlock(IByteData data, int id, string name, int address, int blockX, int blockY)
        : base(data, id, name, address, 6 * c_meshCount) {
            BlockX = blockX;
            BlockY = blockY;

            var pos = Address;
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
                    new FIXED(Data.GetWeirdCompressedFIXED(normalAddresses[x, y] + 0)),
                    new FIXED(Data.GetWeirdCompressedFIXED(normalAddresses[x, y] + 2)),
                    new FIXED(Data.GetWeirdCompressedFIXED(normalAddresses[x, y] + 4))
                );
            }
            set {
                Data.SetWeirdCompressedFIXED(normalAddresses[x, y] + 0, new CompressedFIXED(value.X));
                Data.SetWeirdCompressedFIXED(normalAddresses[x, y] + 2, new CompressedFIXED(value.Y));
                Data.SetWeirdCompressedFIXED(normalAddresses[x, y] + 4, new CompressedFIXED(value.Z));
            }
        }

        public int BlockNum => ID;

        [TableViewModelColumn(displayOrder: 0, isReadOnly: true, displayFormat: "D2")]
        public int BlockX { get; }

        [TableViewModelColumn(displayOrder: 1, isReadOnly: true, displayFormat: "D2")]
        public int BlockY { get; }

        // TODO: Let's make this actually editable I guess?
        [TableViewModelColumn(displayOrder: 2, isReadOnly: true, displayName: "Normals", minWidth: 1500)]
        public string AllNormals {
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
