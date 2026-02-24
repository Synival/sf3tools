using System.Linq;
using CommonLib;
using SF3.MPD.Interfaces;
using SF3.Types;
using SF3.Win.Extensions;
using static SF3.Win.Controls.MPD_ViewerGLControl;

namespace SF3.Win.OpenGL.MPD {
    public class EditorResources : ResourcesBase {
        protected override void PerformInit() {
            Textures = [];
            Models = [];
        }

        public override void DeInit() {
            Models?.Dispose();
            Textures?.Dispose();

            _mouseoverTileModel   = null;
            _selectedTileModel    = null;

            _mouseoverTileTexture = null;
            _selectedTileTexture  = null;

            Models   = null;
            Textures = null;
        }

        public override void Reset() {
            // Nothing loaded dynamically, so nothing to reset.
        }

        public void UpdateMouseoverObject(IMPD mpd, GeneralResources world, ISelectableObject obj) {
            ReplaceTileModelAndTexture(mpd, world, obj as SelectableTile, ref _mouseoverTileModel, ref _mouseoverTileTexture);
            MouseoverObject = obj;
        }

        public void UpdateSelectedObject(IMPD mpd, GeneralResources world, ISelectableObject obj) {
            ReplaceTileModelAndTexture(mpd, world, obj as SelectableTile, ref _selectedTileModel, ref _selectedTileTexture);
            SelectedObject = obj;
        }

        private void ReplaceTileModelAndTexture(IMPD mpd, GeneralResources world, SelectableTile selectableTile, ref QuadModel model, ref Texture texture) {
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

            if (selectableTile != null) {
                var tile = mpd.Surface.GetTile(selectableTile.X, selectableTile.Y);

                var texId = tile.TextureID;
                var tileTexture = (texId == 0xFF) ? null : mpd.ModelCollections[MPD_CollectionType.Primary].Textures.FirstOrDefault(x => x.ID == texId);
                if (tileTexture != null) 
                    Textures.Add(texture = new Texture(tileTexture.CreateBitmapARGB8888()));

                var quad = new Quad(tile.GetVector3Vertices());
                Models.Add(model = new QuadModel([quad]));
            }
        }

        private QuadModel _mouseoverTileModel = null;
        private Texture _mouseoverTileTexture = null;
        public QuadModel MouseoverTileModel => _mouseoverTileModel;
        public Texture MouseoverTileTexture => _mouseoverTileTexture;

        private QuadModel _selectedTileModel = null;
        private Texture _selectedTileTexture = null;
        public QuadModel SelectedTileModel => _selectedTileModel;
        public Texture SelectedTileTexture => _selectedTileTexture;

        public ISelectableObject MouseoverObject { get; private set; }
        public ISelectableObject SelectedObject { get; private set; }

        public DisposableList<QuadModel> Models { get; private set; } = null;
        public DisposableList<Texture> Textures { get; private set; } = null;
    }
}
