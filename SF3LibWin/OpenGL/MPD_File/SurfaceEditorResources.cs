using System;
using System.Drawing;
using CommonLib;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.Extensions;
using SF3.Win.Properties;

namespace SF3.Win.OpenGL.MPD_File {
    public class SurfaceEditorResources : IDisposable {
        private bool _isInitialized = false;
        public void Init() {
            if (_isInitialized)
                return;
            _isInitialized = true;

            Textures = [
                (TileHoverTexture    = new Texture(Resources.TileHoverBmp)),
                (TileSelectedTexture = new Texture(Resources.TileSelectedBmp)),
                (HelpTexture         = new Texture(Resources.ViewerHelpBmp)),
            ];

            var helpWidth = HelpTexture.Width / HelpTexture.Height;
            Models = [
                (HelpModel = new QuadModel([
                    new Quad([
                        new Vector3(-helpWidth,  0, 0),
                        new Vector3(         0,  0, 0),
                        new Vector3(         0,  1, 0),
                        new Vector3(-helpWidth,  1, 0)
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

                TileHoverModel = null;
                TileSelectedModel = null;
                HelpModel = null;

                TileHoverTexture = null;
                TileSelectedTexture = null;
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

        public void UpdateTileHoverModel(IMPD_File mpdFile, WorldResources world, Point? tilePos) {
            TileHoverModel?.Dispose();
            if (TileHoverModel != null)
                Models.Remove(TileHoverModel);
            TileHoverModel = null;

            if (tilePos != null) {
                var tile = mpdFile.Tiles[tilePos.Value.X, tilePos.Value.Y];
                var quad = new Quad(tile.GetSurfaceModelVertices());
                Models.Add(TileHoverModel = new QuadModel([quad]));
            }
        }

        public void UpdateTileSelectedModel(IMPD_File mpdFile, WorldResources world, Point? tilePos) {
            TileSelectedModel?.Dispose();
            if (TileSelectedModel != null)
                Models.Remove(TileSelectedModel);
            TileSelectedModel = null;

            if (tilePos != null) {
                var tile = mpdFile.Tiles[tilePos.Value.X, tilePos.Value.Y];
                var quad = new Quad(tile.GetSurfaceModelVertices(2.00f));
                Models.Add(TileSelectedModel = new QuadModel([quad]));
            }
        }

        public QuadModel TileHoverModel { get; private set; } = null;
        public QuadModel TileSelectedModel { get; private set; } = null;
        public QuadModel HelpModel { get; private set; } = null;

        public Texture TileHoverTexture { get; private set; } = null;
        public Texture TileSelectedTexture { get; private set; } = null;
        public Texture HelpTexture { get; private set; } = null;

        public DisposableList<QuadModel> Models { get; private set; } = null;
        public DisposableList<Texture> Textures { get; private set; } = null;
    }
}
