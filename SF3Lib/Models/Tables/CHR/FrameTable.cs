using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;
using SF3.Types;

namespace SF3.Models.Tables.CHR {
    public class FrameTable : TerminatedTable<Frame> {
        protected FrameTable(IByteData data, string name, int address, uint dataOffset, int width, int height, string rowPrefix, int spriteId, int directions)
        : base(data, name, address, 0x04, 100) {
            DataOffset = dataOffset;
            Width      = width;
            Height     = height;
            RowPrefix  = rowPrefix ?? "";
            SpriteID   = spriteId;
            Directions = directions;
        }

        public static FrameTable Create(IByteData data, string name, int address, uint dataOffset, int width, int height, string rowPrefix, int spriteId, int directions) {
            var newTable = new FrameTable(data, name, address, dataOffset, width, height, rowPrefix, spriteId, directions);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new Frame(Data, id, $"{RowPrefix}Frame{id:D2}", addr, DataOffset, Width, Height, SpriteID, IdToDirection(id)),
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
                        case 0:  return SpriteFrameDirection.SSE;
                        case 1:  return SpriteFrameDirection.ESE;
                        case 2:  return SpriteFrameDirection.ENE;
                        default: return SpriteFrameDirection.NNE;
                    }

                case 5:
                    switch (id % 5) {
                        case 0:  return SpriteFrameDirection.S;
                        case 1:  return SpriteFrameDirection.SE;
                        case 2:  return SpriteFrameDirection.E;
                        case 3:  return SpriteFrameDirection.NE;
                        default: return SpriteFrameDirection.N;
                    }

                case 6:
                    switch (id % 6) {
                        case 0:  return SpriteFrameDirection.SSE;
                        case 1:  return SpriteFrameDirection.ESE;
                        case 2:  return SpriteFrameDirection.ENE;
                        case 3:  return SpriteFrameDirection.NNE;
                        case 4:  return SpriteFrameDirection.S;
                        default: return SpriteFrameDirection.N;
                    }

                case 8:
                    switch (id % 8) {
                        case 0:  return SpriteFrameDirection.SSE;
                        case 1:  return SpriteFrameDirection.ESE;
                        case 2:  return SpriteFrameDirection.ENE;
                        case 3:  return SpriteFrameDirection.NNE;
                        case 4:  return SpriteFrameDirection.NNW;
                        case 5:  return SpriteFrameDirection.WNW;
                        case 6:  return SpriteFrameDirection.WSW;
                        default: return SpriteFrameDirection.SSW;
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
        public int SpriteID { get; }
        public int Directions { get; }
    }
}
