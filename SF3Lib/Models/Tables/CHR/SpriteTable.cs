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
            int nextIdInGroup = 0;

            return Load(
                (id, addr) => {
                    var spr = new Sprite(Data, id, nextIdInGroup++, $"{nameof(Sprite)}{id:D2}", IsCHP ? (int) nextAddr : addr, currentDataOffset, NameGetterContext);
                    nextAddr += (uint) spr.Size;

                    if (IsCHP) {
                        while (nextAddr < Data.Length - 0x18 && !spr.Header.IsValid()) {
                            nextAddr = (nextAddr & 0x7FFFF800) + 0x800;
                            currentDataOffset = nextAddr;
                            if (currentDataOffset >= Data.Length - 0x18) {
                                keepGoing = false;
                                return null;
                            }
                            spr = new Sprite(Data, id, 0, $"{nameof(Sprite)}{id:D2}", (int) nextAddr, currentDataOffset, NameGetterContext);
                        }
                        nextAddr += 0x18;
                        nextIdInGroup = 1;
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
