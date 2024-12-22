using System.Collections.Generic;
using System.Linq;

namespace SF3.Win.OpenGL {
    public class VBO : Buffer {
        /// <summary>
        /// Vertex Buffer Object.
        /// </summary>
        /// <param name="attributes">The attributes that this VBO should have.</param>
        public VBO(VBO_Attribute[] attributes) {
            Attributes = attributes;
            _attributesByName = Attributes.ToDictionary(x => x.Name);

            var off = 0;
            foreach (var attr in Attributes) {
                attr.OffsetInBytes = off;
                off += attr.SizeInBytes;
            }

            StrideInBytes = off;
        }

        public VBO_Attribute GetAttributeByName(string name)
            => _attributesByName.TryGetValue(name, out var value) ? value : null;

        public int GetSizeInBytes(int vertices)
            => StrideInBytes * vertices;

        public VBO_Attribute[] Attributes { get; }
        public int StrideInBytes { get; }

        private readonly Dictionary<string, VBO_Attribute> _attributesByName;
    }
}
