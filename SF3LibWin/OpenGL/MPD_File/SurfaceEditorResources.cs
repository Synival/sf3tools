using System.Drawing;
using CommonLib;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.Extensions;
using SF3.Win.Properties;
using static CommonLib.Types.CornerTypeConsts;

namespace SF3.Win.OpenGL.MPD_File {
    public class SurfaceEditorResources : ResourcesBase {
        protected override void PerformInit() {
            Textures = [
                (TileHoverTexture    = new Texture(Resources.TileHoverBmp)),
                (TileSelectedTexture = new Texture(Resources.TileSelectedBmp)),
                (HelpTexture         = new Texture(Resources.ViewerHelpBmp)),
            ];

            var helpWidth = HelpTexture.Width / HelpTexture.Height;
            Models = [
                (HelpModel = new QuadModel([
                    new Quad([
                        new Vector3(helpWidth * (Corner1X - 1),  Corner1Y, 0),
                        new Vector3(helpWidth * (Corner2X - 1),  Corner2Y, 0),
                        new Vector3(helpWidth * (Corner3X - 1),  Corner3Y, 0),
                        new Vector3(helpWidth * (Corner4X - 1),  Corner4Y, 0)
                    ])
                ]))
            ];
        }

        public override void DeInit() {
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

        public override void Reset() {
            // Nothing loaded dynamically, so nothing to reset.
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
