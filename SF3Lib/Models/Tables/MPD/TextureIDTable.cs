using System;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class TextureIDTable : TerminatedTable<TextureIDModel> {
        protected TextureIDTable(IByteData data, string name, int address) : base(data, name, address, 2, null) {
        }

        public static TextureIDTable Create(IByteData data, string name, int address) {
            var newTable = new TextureIDTable(data, name, address);
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
