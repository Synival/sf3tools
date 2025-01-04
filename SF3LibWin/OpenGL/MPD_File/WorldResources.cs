using System;
using System.Drawing;
using CommonLib;

namespace SF3.Win.OpenGL.MPD_File {
    public class WorldResources : IDisposable {
        public const float ModelOffsetX = SurfaceModelResources.WidthInTiles / -2f;
        public const float ModelOffsetZ = SurfaceModelResources.HeightInTiles / -2f;

        private bool _isInitialized = false;
        public void Init() {
            if (_isInitialized)
                return;
            _isInitialized = true;

            Shaders = [
                (TextureShader    = new Shader("Shaders/Texture.vert",    "Shaders/Texture.frag")),
                (TwoTextureShader = new Shader("Shaders/TwoTexture.vert", "Shaders/TwoTexture.frag")),
                (SolidShader      = new Shader("Shaders/Solid.vert",      "Shaders/Solid.frag")),
                (NormalsShader    = new Shader("Shaders/Normals.vert",    "Shaders/Normals.frag")),
                (WireframeShader  = new Shader("Shaders/Wireframe.vert",  "Shaders/Wireframe.frag")),
                (ObjectShader     = new Shader("Shaders/Object.vert",     "Shaders/Object.frag")),
            ];

            Textures = [
                (WhiteTexture         = new Texture((Bitmap) Image.FromFile("Images/White.bmp"))),
                (TransparentTexture   = new Texture((Bitmap) Image.FromFile("Images/Transparent.bmp"))),
                (TileWireframeTexture = new Texture((Bitmap) Image.FromFile("Images/TileWireframe.bmp"))),
            ];
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                Shaders?.Dispose();
                Textures?.Dispose();

                TextureShader = null;
                TwoTextureShader = null;
                SolidShader = null;
                NormalsShader = null;
                WireframeShader = null;
                ObjectShader = null;

                WhiteTexture = null;
                TransparentTexture = null;
                TileWireframeTexture = null;

                Shaders = null;
                Textures = null;
            }

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~WorldResources() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine(GetType().Name + ": GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public Shader TextureShader { get; private set; } = null;
        public Shader TwoTextureShader { get; private set; } = null;
        public Shader SolidShader { get; private set; } = null;
        public Shader NormalsShader { get; private set; } = null;
        public Shader WireframeShader { get; private set; } = null;
        public Shader ObjectShader { get; private set; } = null;

        public Texture TileWireframeTexture { get; private set; } = null;
        public Texture WhiteTexture { get; private set; } = null;
        public Texture TransparentTexture { get; private set; } = null;

        public DisposableList<Shader> Shaders { get; private set; } = null;
        public DisposableList<Texture> Textures { get; private set; } = null;
    }
}
