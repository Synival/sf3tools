using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.Shared {
    public class TileMovement : Struct {
        private readonly int noEntry;
        private readonly int air;
        private readonly int grassland;
        private readonly int dirt;
        private readonly int darkGrass;
        private readonly int forest;
        private readonly int brownMountain;
        private readonly int desert;
        private readonly int greyMountain;
        private readonly int water;
        private readonly int cantStay;
        private readonly int sand;
        private readonly int enemyOnly;
        private readonly int playerOnly;
        private readonly int unknown0e;
        private readonly int unknown0f;

        public TileMovement(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10) {
            noEntry       = Address;
            air     = Address + 0x01;
            grassland     = Address + 0x02;
            dirt          = Address + 0x03;
            darkGrass     = Address + 0x04;
            forest        = Address + 0x05;
            brownMountain = Address + 0x06;
            desert        = Address + 0x07;
            greyMountain  = Address + 0x08;
            water         = Address + 0x09;
            cantStay      = Address + 0x0a;
            sand          = Address + 0x0b;
            enemyOnly     = Address + 0x0c;
            playerOnly    = Address + 0x0d;
            unknown0e     = Address + 0x0e;
            unknown0f     = Address + 0x0f;
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public int NoEntry {
            get => Data.GetByte(noEntry);
            set => Data.SetByte(noEntry, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int Air {
            get => Data.GetByte(air);
            set => Data.SetByte(air, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public int Grassland {
            get => Data.GetByte(grassland);
            set => Data.SetByte(grassland, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public int Dirt {
            get => Data.GetByte(dirt);
            set => Data.SetByte(dirt, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public int DarkGrass {
            get => Data.GetByte(darkGrass);
            set => Data.SetByte(darkGrass, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 5, displayFormat: "X2")]
        [BulkCopy]
        public int Forest {
            get => Data.GetByte(forest);
            set => Data.SetByte(forest, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 6, displayFormat: "X2")]
        [BulkCopy]
        public int BrownMountain {
            get => Data.GetByte(brownMountain);
            set => Data.SetByte(brownMountain, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 7, displayFormat: "X2")]
        [BulkCopy]
        public int Desert {
            get => Data.GetByte(desert);
            set => Data.SetByte(desert, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 8, displayFormat: "X2")]
        [BulkCopy]
        public int GreyMountain {
            get => Data.GetByte(greyMountain);
            set => Data.SetByte(greyMountain, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 9, displayFormat: "X2")]
        [BulkCopy]
        public int Water {
            get => Data.GetByte(water);
            set => Data.SetByte(water, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 10, displayFormat: "X2")]
        [BulkCopy]
        public int CantStay {
            get => Data.GetByte(cantStay);
            set => Data.SetByte(cantStay, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 11, displayFormat: "X2")]
        [BulkCopy]
        public int Sand {
            get => Data.GetByte(sand);
            set => Data.SetByte(sand, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 12, displayFormat: "X2")]
        [BulkCopy]
        public int EnemyOnly {
            get => Data.GetByte(enemyOnly);
            set => Data.SetByte(enemyOnly, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 13, displayFormat: "X2")]
        [BulkCopy]
        public int PlayerOnly {
            get => Data.GetByte(playerOnly);
            set => Data.SetByte(playerOnly, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 14, displayFormat: "X2")]
        [BulkCopy]
        public int UnknownE {
            get => Data.GetByte(unknown0e);
            set => Data.SetByte(unknown0e, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 15, displayFormat: "X2")]
        [BulkCopy]
        public int UnknownF {
            get => Data.GetByte(unknown0f);
            set => Data.SetByte(unknown0f, (byte) value);
        }
    }
}
