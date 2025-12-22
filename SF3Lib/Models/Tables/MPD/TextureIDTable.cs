using System;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class TextureIDTable : TerminatedTable<TextureIDStruct> {
        protected TextureIDTable(IByteData data, string name, string itemPrefix, int address, int terminatorSize, int? maxSize) : base(data, name, address, terminatorSize, maxSize) {
            ItemPrefix = itemPrefix;
        }

        public static TextureIDTable Create(IByteData data, string name, string itemPrefix, int address, int terminatorSize, int? maxSize)
            => Create(() => new TextureIDTable(data, name, itemPrefix, address, terminatorSize, maxSize));

        public override bool Load() {
            return Load(
                (id, address) => new TextureIDStruct(Data, id, $"{ItemPrefix}_{id:D2}", address),
                (currentRows, model) => model.TextureID != 0xFFFF, addEndModel: false);
        }

        public string ItemPrefix { get; }
    }
}
