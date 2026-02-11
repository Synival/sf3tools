using System.Drawing;
using CommonLib;
using SF3.MPD.Interfaces;
using SF3.Win.Extensions;
using SF3.Win.Properties;

namespace SF3.Win.OpenGL.MPD {
    public class SurfaceEditorResources : ResourcesBase {
        protected override void PerformInit() {
            Textures = [
                (TileHoverTexture    = new Texture(Resources.TileHoverBmp)),
                (TileSelectedTexture = new Texture(Resources.TileSelectedBmp)),
            ];

            Models = [];
        }

        public override void DeInit() {
            Models?.Dispose();
            Textures?.Dispose();

            TileHoverModel = null;
            TileSelectedModel = null;

            TileHoverTexture = null;
            TileSelectedTexture = null;

            Models = null;
            Textures = null;
        }

        public override void Reset() {
            // Nothing loaded dynamically, so nothing to reset.
        }

        public void UpdateTileHoverModel(IMPD mpd, GeneralResources world, Point? tilePos) {
            TileHoverModel?.Dispose();
            if (TileHoverModel != null)
                Models.Remove(TileHoverModel);
            TileHoverModel = null;

            if (tilePos != null) {
                var tile = mpd.Surface.GetTile(tilePos.Value.X, tilePos.Value.Y);
                var quad = new Quad(tile.GetVector3Vertices());
                Models.Add(TileHoverModel = new QuadModel([quad]));
            }
        }

        public void UpdateTileSelectedModel(IMPD mpd, GeneralResources world, Point? tilePos) {
            TileSelectedModel?.Dispose();
            if (TileSelectedModel != null)
                Models.Remove(TileSelectedModel);
            TileSelectedModel = null;

            if (tilePos != null) {
                var tile = mpd.Surface.GetTile(tilePos.Value.X, tilePos.Value.Y);
                var quad = new Quad(tile.GetVector3Vertices(2.00f));
                Models.Add(TileSelectedModel = new QuadModel([quad]));
            }
        }

        public QuadModel TileHoverModel { get; private set; } = null;
        public QuadModel TileSelectedModel { get; private set; } = null;

        public Texture TileHoverTexture { get; private set; } = null;
        public Texture TileSelectedTexture { get; private set; } = null;

        public DisposableList<QuadModel> Models { get; private set; } = null;
        public DisposableList<Texture> Textures { get; private set; } = null;
    }
}
