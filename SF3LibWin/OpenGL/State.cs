using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class State {
        // TODO: This should be referenced by the current OpenTX context!!!
        // TODO: ...or maybe all textures and stuff should be attached here...?
        private static readonly State _state = new State();
        public static State GetCurrentState()
            => _state;

        public int ShaderHandle { get; set; } = 0;
        public TextureUnit ActiveTexture { get; internal set; }
        public int TextureHandle { get; set; } = 0;
        // TODO: needs separate stats per framebuffer target!!
        public int FramebufferHandle { get; set; } = 0;
        // TODO: needs separate stats per buffer target!!
        public int BufferHandle { get; set; } = 0;
        public int VertexArrayHandle { get; set; } = 0;
    }
}
