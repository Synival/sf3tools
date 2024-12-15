﻿using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.MPD {
    public class TextureAnimationTable : Table<TextureAnimationModel> {
        public TextureAnimationTable(IByteData data, int address, bool is32Bit) : base(data, address) {
            Is32Bit       = is32Bit;
            _frameEndId   = is32Bit ? 0xFFFF_FFFE : 0xFFFE;
            _textureEndId = is32Bit ? 0xFFFF_FFFF : 0xFFFF;
        }

        public override bool Load() {
            return LoadUntilMax(
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

        private uint _frameEndId;
        private uint _textureEndId;
    }
}
