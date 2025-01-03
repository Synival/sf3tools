using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class VBO_Attribute : BaseAttribute {
        public VBO_Attribute(int elements, ActiveAttribType type, string name)
        : base(elements, type, name) { }

        public int? OffsetInBytes { get; set; } = null;
    }
}
