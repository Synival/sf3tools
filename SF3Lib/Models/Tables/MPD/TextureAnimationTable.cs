using System;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class TextureAnimationTable : TerminatedTable<TextureAnimationModel> {
        protected TextureAnimationTable(IByteData data, string name, int address, bool is32Bit) : base(data, name, address, is32Bit ? 4 : 2, null) {
            Is32Bit       = is32Bit;
            _frameEndId   = is32Bit ? 0xFFFF_FFFE : 0xFFFE;
            _textureEndId = is32Bit ? 0xFFFF_FFFF : 0xFFFF;
        }

        public static TextureAnimationTable Create(IByteData data, string name, int address, bool is32Bit) {
            var newTable = new TextureAnimationTable(data, name, address, is32Bit);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, address) => {
                    // For some reason, Scenario 2's SARA23.MPD's final animation has an ID of 0xFFFE instead of 0xFFFF like
                    // everything else. No clue why, but let's consider that the end as well.
                    var textureId = Data.GetData(address, Is32Bit ? 4 : 2);
                    var atEnd = textureId == _frameEndId || textureId == _textureEndId;
                    return new TextureAnimationModel(Data, id, atEnd ? "--" : "TexAnim" + id, address, Is32Bit);
                },
                (currentRows, model) => model.TextureID != _frameEndId && model.TextureID != _textureEndId, addEndModel: false);
        }

        public bool Is32Bit { get; }

        private readonly uint _frameEndId;
        private readonly uint _textureEndId;
    }
}
