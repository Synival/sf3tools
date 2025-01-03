using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.ByteData;

namespace SF3.Models.Structs.Shared {
    public class TileMovement : Struct {
        private readonly int noEntry;
        private readonly int unknown01;
        private readonly int grassland;
        private readonly int dirt;
        private readonly int darkGrass;
        private readonly int forest;
        private readonly int brownMountain;
        private readonly int desert;
        private readonly int greyMountain;

        private readonly int unknown09;
        private readonly int unknown0a;
        private readonly int unknown0b;
        private readonly int unknown0c;
        private readonly int unknown0d;
        private readonly int unknown0e;
        private readonly int unknown0f;

        public TileMovement(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10) {
            noEntry       = Address;
            unknown01     = Address + 0x01;
            grassland     = Address + 0x02;
            dirt          = Address + 0x03;
            darkGrass     = Address + 0x04;
            forest        = Address + 0x05;
            brownMountain = Address + 0x06;
            desert        = Address + 0x07;
            greyMountain  = Address + 0x08;

            unknown09     = Address + 0x09;
            unknown0a     = Address + 0x0a;
            unknown0b     = Address + 0x0b;
            unknown0c     = Address + 0x0c;
            unknown0d     = Address + 0x0d;
            unknown0e     = Address + 0x0e;
            unknown0f     = Address + 0x0f;
        }

        [BulkCopy]
        public int TileNoEntry {
            get => Data.GetByte(noEntry);
            set => Data.SetByte(noEntry, (byte) value);
        }

        [BulkCopy]
        public int TileUnknown1 {
            get => Data.GetByte(unknown01);
            set => Data.SetByte(unknown01, (byte) value);
        }

        [BulkCopy]
        public int TileGrassland {
            get => Data.GetByte(grassland);
            set => Data.SetByte(grassland, (byte) value);
        }

        [BulkCopy]
        public int TileDirt {
            get => Data.GetByte(dirt);
            set => Data.SetByte(dirt, (byte) value);
        }

        [BulkCopy]
        public int TileDarkGrass {
            get => Data.GetByte(darkGrass);
            set => Data.SetByte(darkGrass, (byte) value);
        }

        [BulkCopy]
        public int TileForest {
            get => Data.GetByte(forest);
            set => Data.SetByte(forest, (byte) value);
        }

        [BulkCopy]
        public int TileBrownMountain {
            get => Data.GetByte(brownMountain);
            set => Data.SetByte(brownMountain, (byte) value);
        }

        [BulkCopy]
        public int TileDesert {
            get => Data.GetByte(desert);
            set => Data.SetByte(desert, (byte) value);
        }

        [BulkCopy]
        public int TileGreyMountain {
            get => Data.GetByte(greyMountain);
            set => Data.SetByte(greyMountain, (byte) value);
        }

        [BulkCopy]
        public int TileUnknown9 {
            get => Data.GetByte(unknown09);
            set => Data.SetByte(unknown09, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownA {
            get => Data.GetByte(unknown0a);
            set => Data.SetByte(unknown0a, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownB {
            get => Data.GetByte(unknown0b);
            set => Data.SetByte(unknown0b, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownC {
            get => Data.GetByte(unknown0c);
            set => Data.SetByte(unknown0c, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownD {
            get => Data.GetByte(unknown0d);
            set => Data.SetByte(unknown0d, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownE {
            get => Data.GetByte(unknown0e);
            set => Data.SetByte(unknown0e, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownF {
            get => Data.GetByte(unknown0f);
            set => Data.SetByte(unknown0f, (byte) value);
        }
    }
}
