using System.Collections.Generic;
using System.Linq;

namespace SF3.Win.OpenGL {
    public class VBO : Buffer {
        public VBO(VBO_Attribute[] attributes) {
            Attributes = attributes;
            _attributesByName = Attributes.ToDictionary(x => x.Name);

            var off = 0;
            foreach (var attr in Attributes) {
                attr.OffsetInBytes = off;
                off += attr.SizeInBytes;
            }
        }

        public VBO_Attribute GetAttributeByName(string name)
            => _attributesByName.TryGetValue(name, out var value) ? value : null;

        public VBO_Attribute[] Attributes { get; }
        private readonly Dictionary<string, VBO_Attribute> _attributesByName;
    }
}
