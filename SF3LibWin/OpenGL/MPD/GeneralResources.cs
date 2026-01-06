using CommonLib;
using SF3.Win.Properties;

namespace SF3.Win.OpenGL.MPD_File {
    public class GeneralResources : ResourcesBase {
        public const float ModelOffsetX = SurfaceModelResources.WidthInTiles / -2f;
        public const float ModelOffsetZ = SurfaceModelResources.HeightInTiles / -2f;

        protected override void PerformInit() {
            Shaders = [
                (TextureShader    = new Shader(Resources.TextureVert,    Resources.TextureFrag)),
                (TwoTextureShader = new Shader(Resources.TwoTextureVert, Resources.TwoTextureFrag)),
                (SolidShader      = new Shader(Resources.SolidVert,      Resources.SolidFrag)),
                (NormalsShader    = new Shader(Resources.NormalsVert,    Resources.NormalsFrag)),
                (WireframeShader  = new Shader(Resources.WireframeVert,  Resources.WireframeFrag)),
                (ObjectShader     = new Shader(Resources.ObjectVert,     Resources.ObjectFrag)),
            ];

            Textures = [
                (WhiteTexture            = new Texture(Resources.WhiteBmp)),
                (TransparentWhiteTexture = new Texture(Resources.TransparentWhiteBmp)),
                (TransparentBlackTexture = new Texture(Resources.TransparentBlackBmp)),
                (TileWireframeTexture    = new Texture(Resources.TileWireframeBmp))
            ];
        }

        public override void DeInit() {
            Shaders?.Dispose();
            Textures?.Dispose();

            TextureShader    = null;
            TwoTextureShader = null;
            SolidShader      = null;
            NormalsShader    = null;
            WireframeShader  = null;
            ObjectShader     = null;

            WhiteTexture            = null;
            TransparentWhiteTexture = null;
            TransparentBlackTexture = null;
            TileWireframeTexture    = null;

            Shaders  = null;
            Textures = null;
        }

        public override void Reset() {
            // Nothing dynmically loaded, so nothing to reset.
        }

        public Shader TextureShader { get; private set; } = null;
        public Shader TwoTextureShader { get; private set; } = null;
        public Shader SolidShader { get; private set; } = null;
        public Shader NormalsShader { get; private set; } = null;
        public Shader WireframeShader { get; private set; } = null;
        public Shader ObjectShader { get; private set; } = null;

        public Texture TileWireframeTexture { get; private set; } = null;
        public Texture WhiteTexture { get; private set; } = null;
        public Texture TransparentWhiteTexture { get; private set; } = null;
        public Texture TransparentBlackTexture { get; private set; } = null;

        public DisposableList<Shader> Shaders { get; private set; } = null;
        public DisposableList<Texture> Textures { get; private set; } = null;
    }
}
