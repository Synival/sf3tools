using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL.GLResources.Shared {
    public enum ObjectShaderTextureUnit {
        TextureAtlas    = TextureUnit.Texture4,
        TextureOverlay1 = TextureUnit.Texture5,
        TextureOverlay2 = TextureUnit.Texture6,
        TextureLighting = TextureUnit.Texture7
    }
}
