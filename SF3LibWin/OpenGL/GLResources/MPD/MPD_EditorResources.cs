using System.Collections.Generic;
using System.Linq;
using CommonLib;
using SF3.MPD.Interfaces;
using SF3.Types;
using SF3.Win.Extensions;
using SF3.Win.OpenGL.GLResources.Shared;
using static SF3.Win.Controls.MPD_ViewerGLControl;

namespace SF3.Win.OpenGL.GLResources.MPD {
    public class MPD_EditorResources : ResourcesBase {
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
            var tile = obj as SelectableTile;
            ReplaceTileModelAndTexture(mpd, world, tile == null ? [] : [tile], ref _mouseoverTileModel, ref _mouseoverTileTexture);
            MouseoverObject = obj;
        }

        public void UpdateSelectedObjects(IMPD mpd, GeneralResources world, List<ISelectableObject> objs) {
            ReplaceTileModelAndTexture(mpd, world, objs.Select(x => x as SelectableTile).Where(x => x != null).ToArray(), ref _selectedTileModel, ref _selectedTileTexture);
            SelectedObjects = objs;
        }

        private void ReplaceTileModelAndTexture(IMPD mpd, GeneralResources world, SelectableTile[] selectableTiles, ref QuadModel model, ref Texture texture) {
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

            if (selectableTiles?.Length > 0) {
                var quads = new List<Quad>();
                foreach (var selectableTile in selectableTiles) {
                    var tile = mpd.Surface.GetTile(selectableTile.X, selectableTile.Y);

                    // TODO: This is all wrong!! Build a texture atlas and use atlas texture coordinates.
                    if (texture == null) {
                        var texId = tile.TextureID;
                        var tileTexture = texId == 0xFF ? null : mpd.ModelCollections[MPD_CollectionType.Primary].Textures.FirstOrDefault(x => x.TextureID == texId);
                        if (tileTexture != null) 
                            Textures.Add(texture = new Texture(tileTexture.CreateBitmapARGB8888()));
                    }

                    quads.Add(new Quad(tile.GetVector3Vertices()));
                }
                Models.Add(model = new QuadModel(quads.ToArray()));
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
        public List<ISelectableObject> SelectedObjects { get; private set; }

        public DisposableList<QuadModel> Models { get; private set; } = null;
        public DisposableList<Texture> Textures { get; private set; } = null;
    }
}
