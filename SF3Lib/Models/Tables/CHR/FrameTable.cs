using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;
using SF3.Types;

namespace SF3.Models.Tables.CHR {
    public class FrameTable : TerminatedTable<Frame> {
        protected FrameTable(IByteData data, string name, int address, uint dataOffset, int width, int height, string rowPrefix, int spriteIndex, int spriteId, int directions)
        : base(data, name, address, 0x04, 100) {
            DataOffset = dataOffset;
            Width      = width;
            Height     = height;
            RowPrefix  = rowPrefix ?? "";
            SpriteIndex = spriteIndex;
            SpriteID   = spriteId;
            Directions = directions;
        }

        public static FrameTable Create(IByteData data, string name, int address, uint dataOffset, int width, int height, string rowPrefix, int spriteIndex, int spriteId, int directions) {
            var newTable = new FrameTable(data, name, address, dataOffset, width, height, rowPrefix, spriteIndex, spriteId, directions);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new Frame(Data, id, $"{RowPrefix}Frame{id:D2}", addr, DataOffset, Width, Height, SpriteIndex, SpriteID, IdToDirection(id)),
                (rows, prevRow) => prevRow.TextureOffset != 0,
                addEndModel: false
            );
        }

        private SpriteFrameDirection IdToDirection(int id) {
            switch (Directions) {
                case 1:
                    return SpriteFrameDirection.None;

                case 2:
                    return ((id % 1) == 0) ? SpriteFrameDirection.First : SpriteFrameDirection.Second;

                case 4:
                    switch (id % 4) {
                        case 0:  return SpriteFrameDirection.SSE_4;
                        case 1:  return SpriteFrameDirection.ESE_4;
                        case 2:  return SpriteFrameDirection.ENE_4;
                        default: return SpriteFrameDirection.NNE_4;
                    }

                case 5:
                    switch (id % 5) {
                        case 0:  return SpriteFrameDirection.S_5;
                        case 1:  return SpriteFrameDirection.SE_5;
                        case 2:  return SpriteFrameDirection.E_5;
                        case 3:  return SpriteFrameDirection.NE_5;
                        default: return SpriteFrameDirection.N_5;
                    }

                case 6:
                    switch (id % 6) {
                        case 0:  return SpriteFrameDirection.SSE_6;
                        case 1:  return SpriteFrameDirection.ESE_6;
                        case 2:  return SpriteFrameDirection.ENE_6;
                        case 3:  return SpriteFrameDirection.NNE_6;
                        case 4:  return SpriteFrameDirection.S_6;
                        default: return SpriteFrameDirection.N_6;
                    }

                case 8:
                    switch (id % 8) {
                        case 0:  return SpriteFrameDirection.SSE_8;
                        case 1:  return SpriteFrameDirection.ESE_8;
                        case 2:  return SpriteFrameDirection.ENE_8;
                        case 3:  return SpriteFrameDirection.NNE_8;
                        case 4:  return SpriteFrameDirection.NNW_8;
                        case 5:  return SpriteFrameDirection.WNW_8;
                        case 6:  return SpriteFrameDirection.WSW_8;
                        default: return SpriteFrameDirection.SSW_8;
                    }

                // No known cases of any other directions
                default:
                    return (SpriteFrameDirection) (id % Directions);
            }
        }

        public uint DataOffset { get; }
        public int Width { get; }
        public int Height { get; }
        public string RowPrefix { get; }
        public int SpriteIndex { get; }
        public int SpriteID { get; }
        public int Directions { get; }
    }
}
