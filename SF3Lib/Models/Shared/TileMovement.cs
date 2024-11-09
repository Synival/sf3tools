using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models.Shared {
    public class TileMovement : Model {
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

        public TileMovement(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x10) {
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
            get => Editor.GetByte(noEntry);
            set => Editor.SetByte(noEntry, (byte) value);
        }

        [BulkCopy]
        public int TileUnknown1 {
            get => Editor.GetByte(unknown01);
            set => Editor.SetByte(unknown01, (byte) value);
        }

        [BulkCopy]
        public int TileGrassland {
            get => Editor.GetByte(grassland);
            set => Editor.SetByte(grassland, (byte) value);
        }

        [BulkCopy]
        public int TileDirt {
            get => Editor.GetByte(dirt);
            set => Editor.SetByte(dirt, (byte) value);
        }

        [BulkCopy]
        public int TileDarkGrass {
            get => Editor.GetByte(darkGrass);
            set => Editor.SetByte(darkGrass, (byte) value);
        }

        [BulkCopy]
        public int TileForest {
            get => Editor.GetByte(forest);
            set => Editor.SetByte(forest, (byte) value);
        }

        [BulkCopy]
        public int TileBrownMountain {
            get => Editor.GetByte(brownMountain);
            set => Editor.SetByte(brownMountain, (byte) value);
        }

        [BulkCopy]
        public int TileDesert {
            get => Editor.GetByte(desert);
            set => Editor.SetByte(desert, (byte) value);
        }

        [BulkCopy]
        public int TileGreyMountain {
            get => Editor.GetByte(greyMountain);
            set => Editor.SetByte(greyMountain, (byte) value);
        }

        [BulkCopy]
        public int TileUnknown9 {
            get => Editor.GetByte(unknown09);
            set => Editor.SetByte(unknown09, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownA {
            get => Editor.GetByte(unknown0a);
            set => Editor.SetByte(unknown0a, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownB {
            get => Editor.GetByte(unknown0b);
            set => Editor.SetByte(unknown0b, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownC {
            get => Editor.GetByte(unknown0c);
            set => Editor.SetByte(unknown0c, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownD {
            get => Editor.GetByte(unknown0d);
            set => Editor.SetByte(unknown0d, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownE {
            get => Editor.GetByte(unknown0e);
            set => Editor.SetByte(unknown0e, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownF {
            get => Editor.GetByte(unknown0f);
            set => Editor.SetByte(unknown0f, (byte) value);
        }
    }
}
