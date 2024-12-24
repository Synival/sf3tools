using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class State {
        // TODO: This should be referenced by the current OpenTX context!!!
        // TODO: ...or maybe all textures and stuff should be attached here...?
        private static readonly State _state = new State();
        public static State GetCurrentState()
            => _state;

        public State() {
            var btValues = (BufferTarget[]) Enum.GetValues(typeof(BufferTarget));
            foreach (var value in btValues)
                BufferHandles[value] = 0;

            var tuValues = (TextureUnit[]) Enum.GetValues(typeof(TextureUnit));
            foreach (var value in tuValues)
                TextureHandles[value] = 0;
        }

        public int ShaderHandle { get; set; } = 0;
        public TextureUnit ActiveTexture { get; internal set; }
        public Dictionary<TextureUnit, int> TextureHandles { get; } = new Dictionary<TextureUnit, int>();
        // TODO: needs separate stats per framebuffer target!!
        public int FramebufferHandle { get; set; } = 0;
        public Dictionary<BufferTarget, int> BufferHandles { get; } = new Dictionary<BufferTarget, int>();
        public int VertexArrayHandle { get; set; } = 0;
    }
}
