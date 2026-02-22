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
            if (TileHoverModel != null) {
                TileHoverModel?.Dispose();
                Models.Remove(TileHoverModel);
                TileHoverModel = null;
            }

            if (TileHoverTexture != null) {
                TileHoverTexture?.Dispose();
                Textures.Remove(TileHoverTexture);
                TileHoverTexture = null;
            }

            if (tilePos != null) {
                var tile = mpd.Surface.GetTile(tilePos.Value.X, tilePos.Value.Y);

                var texId = tile.TextureID;
                var texture = (texId == 0xFF) ? null : mpd.ModelCollections[MPD_CollectionType.Primary].Textures.FirstOrDefault(x => x.ID == texId);
                if (texture != null) 
                    Textures.Add(TileHoverTexture = new Texture(texture.CreateBitmapARGB8888()));

                var quad = new Quad(tile.GetVector3Vertices());
                Models.Add(TileHoverModel = new QuadModel([quad]));
            }
        }

        public void UpdateTileSelectedModel(IMPD mpd, GeneralResources world, Point? tilePos) {
            if (TileSelectedModel != null) {
                TileSelectedModel.Dispose();
                Models.Remove(TileSelectedModel);
                TileSelectedModel = null;
            }

            if (TileSelectedTexture != null) {
                TileSelectedTexture.Dispose();
                Textures.Remove(TileSelectedTexture);
                TileSelectedTexture = null;
            }

            if (tilePos != null) {
                var tile = mpd.Surface.GetTile(tilePos.Value.X, tilePos.Value.Y);

                var texId = tile.TextureID;
                var texture = (texId == 0xFF) ? null : mpd.ModelCollections[MPD_CollectionType.Primary].Textures.FirstOrDefault(x => x.ID == texId);
                if (texture != null) 
                    Textures.Add(TileSelectedTexture = new Texture(texture.CreateBitmapARGB8888()));

                var quad = new Quad(tile.GetVector3Vertices());
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
