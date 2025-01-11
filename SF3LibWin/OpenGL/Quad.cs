using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using SF3.Win.Extensions;
using static CommonLib.Extensions.ArrayExtensions;

namespace SF3.Win.OpenGL {
    public class Quad {
        private static readonly Vector4 c_white = new Vector4(1, 1, 1, 1);
        private static readonly Vector4[] c_allWhite = [c_white, c_white, c_white, c_white];

        public Quad(Vector3[] vertices) : this(vertices, null, 0, c_white) { }
        public Quad(Vector3[] vertices, Vector4 color) : this(vertices, null, 0, [color, color, color, color]) { }
        public Quad(Vector3[] vertices, Vector4[] colors) : this(vertices, null, 0, colors) { }
        public Quad(Vector3[] vertices, TextureAnimation textureAnim, byte textureFlags) : this(vertices, textureAnim, textureFlags, c_allWhite) { }
        public Quad(Vector3[] vertices, TextureAnimation textureAnim, byte textureFlags, Vector4 color)
            : this(vertices, textureAnim, textureFlags, [color, color, color, color]) { }

        public Quad(Vector3[] vertices, TextureAnimation textureAnim, byte textureFlags, Vector4[] colors) {
            if (vertices == null || vertices.Length != 4)
                throw new ArgumentException(nameof(vertices));
            if (colors == null || colors.Length != 4)
                throw new ArgumentException(nameof(colors));

            TextureAnim  = textureAnim;
            TextureFlags = textureFlags;

            Attributes = [
                new PolyAttribute(1, OpenTK.Graphics.OpenGL.ActiveAttribType.FloatVec3, "position", 4,
                    vertices.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 3)),
                new PolyAttribute(1, OpenTK.Graphics.OpenGL.ActiveAttribType.FloatVec4, "color", 4,
                    colors.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 4)),
            ];
            _attributesByName = Attributes.ToDictionary(x => x.Name);
        }

        public void AddAttribute(PolyAttribute attr) {
            if (_attributesByName.ContainsKey(attr.Name))
                throw new ArgumentException(nameof(attr.Name));
            Attributes.Add(attr);
            _attributesByName.Add(attr.Name, attr);
        }

        public PolyAttribute GetAttributeByName(string name)
            => _attributesByName.TryGetValue(name, out var value) ? value : null;

        public List<PolyAttribute> Attributes { get; }
        private Dictionary<string, PolyAttribute> _attributesByName;

        public TextureAnimation TextureAnim { get; }
        public byte TextureFlags { get; }
    }
}
