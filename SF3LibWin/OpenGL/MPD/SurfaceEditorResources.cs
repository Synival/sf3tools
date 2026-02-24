using System.Drawing;
using System.Linq;
using CommonLib;
using SF3.MPD.Interfaces;
using SF3.Types;
using SF3.Win.Extensions;

namespace SF3.Win.OpenGL.MPD {
    public class SurfaceEditorResources : ResourcesBase {
        protected override void PerformInit() {
            Textures = [];
            Models = [];
        }

        public override void DeInit() {
            Models?.Dispose();
            Textures?.Dispose();

            _tileHoverModel      = null;
            _tileSelectedModel   = null;

            _tileHoverTexture    = null;
            _tileSelectedTexture = null;

            Models = null;
            Textures = null;
        }

        public override void Reset() {
            // Nothing loaded dynamically, so nothing to reset.
        }

        public void UpdateTileHoverModel(IMPD mpd, GeneralResources world, Point? tilePos)
            => ReplaceTileModelAndTexture(mpd, world, tilePos, ref _tileHoverModel, ref _tileHoverTexture);

        public void UpdateTileSelectedModel(IMPD mpd, GeneralResources world, Point? tilePos)
            => ReplaceTileModelAndTexture(mpd, world, tilePos, ref _tileSelectedModel, ref _tileSelectedTexture);

        private void ReplaceTileModelAndTexture(IMPD mpd, GeneralResources world, Point? tilePos, ref QuadModel model, ref Texture texture) {
            if (model != null) {
                model.Dispose();
                Models.Remove(model);
                model = null;
            }

            if (texture != null) {
                texture.Dispose();
                Textures.Remove(texture);
                texture = null;
            }

            if (tilePos != null) {
                var tile = mpd.Surface.GetTile(tilePos.Value.X, tilePos.Value.Y);

                var texId = tile.TextureID;
                var tileTexture = (texId == 0xFF) ? null : mpd.ModelCollections[MPD_CollectionType.Primary].Textures.FirstOrDefault(x => x.ID == texId);
                if (tileTexture != null) 
                    Textures.Add(texture = new Texture(tileTexture.CreateBitmapARGB8888()));

                var quad = new Quad(tile.GetVector3Vertices());
                Models.Add(model = new QuadModel([quad]));
            }
        }

        private QuadModel _tileHoverModel = null;
        private Texture _tileHoverTexture = null;
        public QuadModel TileHoverModel => _tileHoverModel;
        public Texture TileHoverTexture => _tileHoverTexture;

        private QuadModel _tileSelectedModel = null;
        private Texture _tileSelectedTexture = null;
        public QuadModel TileSelectedModel => _tileSelectedModel;
        public Texture TileSelectedTexture => _tileSelectedTexture;

        public DisposableList<QuadModel> Models { get; private set; } = null;
        public DisposableList<Texture> Textures { get; private set; } = null;
    }
}
