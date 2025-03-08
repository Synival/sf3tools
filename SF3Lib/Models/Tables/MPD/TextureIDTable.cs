using System;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class TextureIDTable : TerminatedTable<TextureIDModel> {
        protected TextureIDTable(IByteData data, string name, int address, int terminatorSize, int? maxSize) : base(data, name, address, terminatorSize, maxSize) {
        }

        public static TextureIDTable Create(IByteData data, string name, int address, int terminatorSize, int? maxSize) {
            var newTable = new TextureIDTable(data, name, address, terminatorSize, maxSize);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, address) => new TextureIDModel(Data, id, "TexAnimAlt" + id, address),
                (currentRows, model) => model.TextureID != 0xFFFF, addEndModel: false);
        }

        public IEnumerable<object> Where(Func<object, bool> value) => throw new NotImplementedException();
    }
}
