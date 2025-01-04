using System;
using System.Drawing;
using CommonLib;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;

namespace SF3.Win.OpenGL.MPD_File {
    public class SurfaceEditorResources {
        private bool _isInitialized = false;
        public void Init() {
            if (_isInitialized)
                return;
            _isInitialized = true;

            Textures = [
                (TileHoverTexture = new Texture((Bitmap) Image.FromFile("Images/TileHover.bmp"))),
                (HelpTexture      = new Texture((Bitmap) Image.FromFile("Images/ViewerHelp.bmp"))),
            ];

            var helpWidth = HelpTexture.Width / HelpTexture.Height;
            Models = [
                (HelpModel = new QuadModel([
                    new Quad([
                        new Vector3(-helpWidth,  1, 0),
                        new Vector3(         0,  1, 0),
                        new Vector3(         0,  0, 0),
                        new Vector3(-helpWidth,  0, 0)
                    ])
                ]))
            ];
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                Models?.Dispose();
                Textures?.Dispose();

                TileModel = null;
                HelpModel = null;

                TileHoverTexture = null;
                HelpTexture = null;

                Models = null;
                Textures = null;
            }

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SurfaceEditorResources() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine(GetType().Name + ": GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public void UpdateTileModel(IMPD_File model, WorldResources world, Point? tilePos) {
            TileModel?.Dispose();
            if (TileModel != null)
                Models.Remove(TileModel);
            TileModel = null;

            if (tilePos != null) {
                var quad = new Quad(SurfaceModelResources.GetTileVertices(model, tilePos.Value));
                Models.Add(TileModel = new QuadModel([quad]));
            }
        }

        public QuadModel TileModel { get; private set; } = null;
        public QuadModel HelpModel { get; private set; } = null;

        public Texture TileHoverTexture { get; private set; } = null;
        public Texture HelpTexture { get; private set; } = null;

        public DisposableList<QuadModel> Models { get; private set; } = null;
        public DisposableList<Texture> Textures { get; private set; } = null;
    }
}
