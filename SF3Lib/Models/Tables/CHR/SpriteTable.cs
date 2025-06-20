using System;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class SpriteTable : TerminatedTable<Sprite> {
        protected SpriteTable(IByteData data, string name, int address, bool isCHP, INameGetterContext ngc)
        : base(data, name, address, terminatedBytes: isCHP ? 0 : 4, maxSize: 300) {
            IsCHP = isCHP;
            NameGetterContext = ngc;
        }

        public static SpriteTable Create(IByteData data, string name, int address, bool isCHP, INameGetterContext ngc) {
            var newTable = new SpriteTable(data, name, address, isCHP, ngc);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            uint currentDataOffset = (uint) Address;
            uint nextAddr = IsCHP ? 0 : currentDataOffset;
            bool keepGoing = true;

            bool isValidSprite(Sprite sprite) {
                var header = sprite.Header;
                return header.SpriteID != 0xFFFF &&
                       header.SpriteID  < 0x0800 &&
                       header.Width  < 0x0200 &&
                       header.Height < 0x0200 &&
                       header.Width  >      0 &&
                       header.Height >      0 &&
                       header.FrameTableOffset < 0x80000 &&
                       header.AnimationTableOffset < 0x80000 &&
                       header.Scale > 0x00500 &&
                       header.Scale < 0x30000 &&
                       header.Directions > 0;
            }

            return Load(
                (id, addr) => {
                    var spr = new Sprite(Data, id, $"{nameof(Sprite)}{id:D2}", IsCHP ? (int) nextAddr : addr, currentDataOffset, NameGetterContext);
                    nextAddr += (uint) spr.Size;

                    if (IsCHP) {
                        while (nextAddr < Data.Length - 0x18 && !isValidSprite(spr)) {
                            nextAddr = (nextAddr & 0x7FFFF800) + 0x800;
                            currentDataOffset = nextAddr;
                            spr = new Sprite(Data, id, $"{nameof(Sprite)}{id:D2}", (int) nextAddr, currentDataOffset, NameGetterContext);
                        }
                        nextAddr += 0x18;
                        if (nextAddr >= Data.Length - 0x18)
                            keepGoing = false;
                    }

                    return spr;
                },
                (rows, prevRow) => IsCHP ? keepGoing : prevRow.Header.SpriteID != 0xFFFF,
                false
            );
        }

        public bool IsCHP { get; }
        public INameGetterContext NameGetterContext { get; }
    }
}
