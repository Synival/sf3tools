using CommonLib;
using OpenTK.Mathematics;
using SF3.Win.OpenGL.GLResources.MPD;
using SF3.Win.Properties;

namespace SF3.Win.OpenGL.GLResources.Shared {
    public class GeneralResources : ResourcesBase {
        public const float ModelOffsetX = SurfaceModelResources.WidthInTiles / -2f;
        public const float ModelOffsetZ = SurfaceModelResources.HeightInTiles / -2f;

        protected override void PerformInit() {
            Shaders = [
                (TextureShader           = new Shader(Resources.TextureVert,         Resources.TextureFrag)),
                (TwoTextureShader        = new Shader(Resources.TwoTextureVert,      Resources.TwoTextureFrag)),
                (SolidShader             = new Shader(Resources.SolidVert,           Resources.SolidFrag)),
                (NormalsShader           = new Shader(Resources.NormalsVert,         Resources.NormalsFrag)),
                (WireframeShader         = new Shader(Resources.WireframeVert,       Resources.WireframeFrag)),
                (ObjectShader            = new Shader(Resources.ObjectVert,          Resources.ObjectFrag)),
                (SpriteShader            = new Shader(Resources.SpriteVert,          Resources.SpriteFrag)),
                (ColorizeShader          = new Shader(Resources.ColorizeVert,        Resources.ColorizeFrag)),

                (OutlineBlurPassShader   = new Shader(Resources.OutlineBlurPassVert, Resources.OutlineBlurPassFrag)),
                (OutlineToScreenShader   = new Shader(Resources.OutlineToScreenVert, Resources.OutlineToScreenFrag)),
                (ColorToScreenShader     = new Shader(Resources.ColorToScreenVert,   Resources.ColorToScreenFrag)),
            ];

            Textures = [
                (WhiteTexture            = new Texture(Resources.WhiteBmp)),
                (TransparentWhiteTexture = new Texture(Resources.TransparentWhiteBmp)),
                (TransparentBlackTexture = new Texture(Resources.TransparentBlackBmp)),
                (TileWireframeTexture    = new Texture(Resources.TileWireframeBmp))
            ];

            Models = [
                (FullScreenQuad = new QuadModel([new Quad([
                    new Vector3( 1.0f, -1.0f, 0.0f),
                    new Vector3(-1.0f, -1.0f, 0.0f),
                    new Vector3(-1.0f,  1.0f, 0.0f),
                    new Vector3( 1.0f,  1.0f, 0.0f),
                ])]))
            ];
        }

        public override void DeInit() {
            Shaders?.Dispose();
            Textures?.Dispose();
            Models?.Dispose();

            TextureShader           = null;
            TwoTextureShader        = null;
            SolidShader             = null;
            NormalsShader           = null;
            WireframeShader         = null;
            ObjectShader            = null;
            SpriteShader            = null;
            ColorizeShader          = null;

            OutlineBlurPassShader   = null;
            OutlineToScreenShader   = null;
            ColorToScreenShader     = null;

            WhiteTexture            = null;
            TransparentWhiteTexture = null;
            TransparentBlackTexture = null;
            TileWireframeTexture    = null;

            FullScreenQuad = null;

            Shaders  = null;
            Textures = null;
            Models   = null;
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
        public Shader SpriteShader { get; private set; } = null;
        public Shader ColorizeShader { get; private set; } = null;

        public Shader OutlineBlurPassShader { get; private set; } = null;
        public Shader OutlineToScreenShader { get; private set; } = null;
        public Shader ColorToScreenShader { get; private set; } = null;

        public Texture TileWireframeTexture { get; private set; } = null;
        public Texture WhiteTexture { get; private set; } = null;
        public Texture TransparentWhiteTexture { get; private set; } = null;
        public Texture TransparentBlackTexture { get; private set; } = null;

        public QuadModel FullScreenQuad { get; private set; } = null;

        public DisposableList<Shader> Shaders { get; private set; } = null;
        public DisposableList<Texture> Textures { get; private set; } = null;
        public DisposableList<QuadModel> Models { get; private set; } = null;
    }
}
