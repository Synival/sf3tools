using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class SpriteTable : TerminatedTable<Sprite> {
        protected SpriteTable(IByteData data, string name, int address, bool isCHP)
        : base(data, name, address, terminatedBytes: isCHP ? 0 : 4, maxSize: 100) {
            IsCHP = isCHP;
        }

        public static SpriteTable Create(IByteData data, string name, int address, bool isCHP) {
            var newTable = new SpriteTable(data, name, address, isCHP);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            int currentDataOffset = Address;
            int nextAddr = IsCHP ? 0 : Address;
            bool keepGoing = true;

            bool isValidSprite(Sprite spr)
                => spr.SpriteId != 0xFFFF && spr.SpriteId < 0x0800 && spr.Width < 0x0200 && spr.Height < 0x0200 && spr.Width > 0 && spr.Height > 0 &&
                   spr.Offset1 < 0x80000 && spr.Offset2 < 0x80000 && spr.Scale > 0x0500 && spr.Scale < 0x30000 && spr.Directions > 0;

            return Load(
                (id, addr) => {
                    var spr = new Sprite(Data, id, nameof(Sprite) + id.ToString("D2"), IsCHP ? nextAddr : addr, currentDataOffset);
                    nextAddr += spr.Size;

                    if (IsCHP) {
                        while (nextAddr < Data.Length - 0x18 && !isValidSprite(spr)) {
                            nextAddr = (nextAddr & 0x7FFFF800) + 0x800;
                            currentDataOffset = nextAddr;
                            spr = new Sprite(Data, id, nameof(Sprite) + id.ToString("D2"), nextAddr, currentDataOffset);
                        }
                        nextAddr += 0x18;
                        if (nextAddr >= Data.Length - 0x18)
                            keepGoing = false;
                    }

                    return spr;
                },
                (rows, prevRow) => IsCHP ? keepGoing : prevRow.SpriteId != 0xFFFF,
                false
            );
        }

        public bool IsCHP { get; }
    }
}
