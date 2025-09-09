using System.Collections.Generic;
using System.Linq;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class FrameTable : TerminatedTable<Frame> {
        protected FrameTable(IByteData data, string name, int address, uint dataOffset, int width, int height, string rowPrefix, int spriteIndex, int spriteId, int spriteDirections)
        : base(data, name, address, 0x04, 0xF1) {
            DataOffset       = dataOffset;
            Width            = width;
            Height           = height;
            RowPrefix        = rowPrefix ?? "";
            SpriteIndex      = spriteIndex;
            SpriteID         = spriteId;
            SpriteDirections = spriteDirections;
        }

        public static FrameTable Create(IByteData data, string name, int address, uint dataOffset, int width, int height, string rowPrefix, int spriteIndex, int spriteId, int spriteDirections)
            => CreateBase(() => new FrameTable(data, name, address, dataOffset, width, height, rowPrefix, spriteIndex, spriteId, spriteDirections));

        public override bool Load() {
            return Load(
                (id, addr) => new Frame(Data, id, $"{RowPrefix}Frame{id:D2}", addr, DataOffset, Width, Height, SpriteIndex, SpriteID),
                (rows, prevRow) => prevRow.TextureOffset != 0,
                addEndModel: false
            );
        }

        /// <summary>
        /// Fetches a set of sprite names used in this FrameTable, in order from most common to least common.
        /// Can be used resolve ambiguous cases where there are multiple FrameDef's for a given hash.
        /// </summary>
        /// <returns>A HashSet containing the SpriteName of each element in the table, in order from most common to least common.</returns>
        public HashSet<string> GetSingularFrameRefSpriteNames() {
            return new HashSet<string>(this
                .Select(x => x.FrameRefs.GetUniqueSpriteName())
                .Where(x => x != null)
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.First()));
        }

        public uint DataOffset { get; }
        public int Width { get; }
        public int Height { get; }
        public string RowPrefix { get; }
        public int SpriteIndex { get; }
        public int SpriteID { get; }
        public int SpriteDirections { get; }
    }
}
