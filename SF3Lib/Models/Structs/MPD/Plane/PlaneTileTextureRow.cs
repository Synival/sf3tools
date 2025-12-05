using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Plane {
    public class PlaneTileTextureRow : Struct {
        private readonly int[] _xAddress;

        public PlaneTileTextureRow(IByteData data, int id, string name, int address, int blockCountX)
        : base(data, id, name, address, 0x40 * blockCountX * 2) {
            _xAddress = new int[0x40 * blockCountX];
            for (var i = 0; i < _xAddress.Length; i++) {
                var blockX = i / 0x40;
                var xInBlock = i % 0x40;
                _xAddress[i] = Address + (blockX * 0x1000 + xInBlock) * 2;
            }
        }

        public ushort this[int index] {
            get => (ushort) Data.GetWord(_xAddress[index]);
            set => Data.SetWord(_xAddress[index], value);
        }
    }
}
