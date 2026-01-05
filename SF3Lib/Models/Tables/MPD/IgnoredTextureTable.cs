using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class IgnoredTextureTable : TerminatedTable<TextureIDStruct> {
        protected IgnoredTextureTable(IByteData data, string name, int address, int terminatorSize, int? maxSize) : base(data, name, address, terminatorSize, maxSize) {
            // Invalidate the image whenever the texture IDs have been modified.
            data.Data.RangeModified += (s, eventData) => {
                if (eventData.IntersectsWithRange(Address, Address + SizeInBytes))
                    _textureIDs = null;
            };
        }

        public static IgnoredTextureTable Create(IByteData data, string name, int address, int terminatorSize, int? maxSize)
            => Create(() => new IgnoredTextureTable(data, name, address, terminatorSize, maxSize));

        public override bool Load() {
            return Load(
                (id, address) => new TextureIDStruct(Data, id, $"IgnoreTex_{id:D2}", address),
                (currentRows, model) => model.TextureID != 0xFFFF, addEndModel: false);
        }

        private HashSet<int> _textureIDs = null;
        public bool ContainsTextureID(int textureID) {
            if (_textureIDs == null) {
                _textureIDs = new HashSet<int>();
                foreach (var tex in Rows)
                    _textureIDs.Add(tex.TextureID);
            }
            return _textureIDs.Contains(textureID);
        }
    }
}
