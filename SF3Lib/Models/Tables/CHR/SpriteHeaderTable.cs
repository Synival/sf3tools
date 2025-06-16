using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class SpriteHeaderTable : TerminatedTable<SpriteHeader> {
        protected SpriteHeaderTable(IByteData data, string name, int address, bool isCHP)
        : base(data, name, address, terminatedBytes: isCHP ? 0 : 4, maxSize: 100) {
            IsCHP = isCHP;
        }

        public static SpriteHeaderTable Create(IByteData data, string name, int address, bool isCHP) {
            var newTable = new SpriteHeaderTable(data, name, address, isCHP);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            uint currentDataOffset = (uint) Address;
            uint nextAddr = IsCHP ? 0 : currentDataOffset;
            bool keepGoing = true;

            bool isValidSprite(SpriteHeader spr)
                => spr.SpriteID != 0xFFFF && spr.SpriteID < 0x0800 && spr.Width < 0x0200 && spr.Height < 0x0200 && spr.Width > 0 && spr.Height > 0 &&
                   spr.FrameTableOffset < 0x80000 && spr.AnimationTableOffset < 0x80000 && spr.Scale > 0x0500 && spr.Scale < 0x30000 && spr.Directions > 0;

            return Load(
                (id, addr) => {
                    var spr = new SpriteHeader(Data, id, $"Sprite_Header{id:D2}", IsCHP ? (int) nextAddr : addr, currentDataOffset);
                    nextAddr += (uint) spr.Size;

                    if (IsCHP) {
                        while (nextAddr < Data.Length - 0x18 && !isValidSprite(spr)) {
                            nextAddr = (nextAddr & 0x7FFFF800) + 0x800;
                            currentDataOffset = nextAddr;
                            spr = new SpriteHeader(Data, id, $"Sprite_Header{id:D2}", (int) nextAddr, currentDataOffset);
                        }
                        nextAddr += 0x18;
                        if (nextAddr >= Data.Length - 0x18)
                            keepGoing = false;
                    }

                    return spr;
                },
                (rows, prevRow) => IsCHP ? keepGoing : prevRow.SpriteID != 0xFFFF,
                false
            );
        }

        public bool IsCHP { get; }
    }
}
