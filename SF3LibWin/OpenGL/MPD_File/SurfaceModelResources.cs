using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using CommonLib.Types;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.Extensions;
using static CommonLib.Utils.BlockHelpers;

namespace SF3.Win.OpenGL.MPD_File {
    public class SurfaceModelResources {
        public const int WidthInTiles = 64;
        public const int HeightInTiles = 64;

        private bool _isInitialized = false;
        public void Init() {
            if (_isInitialized)
                return;
            _isInitialized = true;

            Models = [];
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing)
                Reset();

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SurfaceModelResources() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine(GetType().Name + ": GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public static Vector3[] GetTileVertices(IMPD_File mpdFile, Point pos) {
            float[] heights;

            // For any tile whose character/texture ID has flag 0x80, the walking heightmap is used.
            if (mpdFile.Surface?.HeightmapRowTable != null && (mpdFile.SurfaceModel?.TileTextureRowTable?.Rows[pos.Y]?.GetTextureFlags(pos.X) & 0x80) == 0x80)
                heights = mpdFile.Surface?.HeightmapRowTable.Rows[pos.Y].GetHeights(pos.X);
            // Otherwise, gather heights from the 5x5 block with the surface mesh's heightmap.
            else if (mpdFile.SurfaceModel?.VertexHeightBlockTable != null) {
                var blockLocations = new BlockVertexLocation[] {
                    GetBlockLocations(pos.X, pos.Y, CornerType.TopLeft,     true)[0],
                    GetBlockLocations(pos.X, pos.Y, CornerType.TopRight,    true)[0],
                    GetBlockLocations(pos.X, pos.Y, CornerType.BottomRight, true)[0],
                    GetBlockLocations(pos.X, pos.Y, CornerType.BottomLeft,  true)[0],
                };

                heights = blockLocations
                    .Select(x => mpdFile.SurfaceModel.VertexHeightBlockTable.Rows[x.Num][x.X, x.Y] / 16.0f)
                    .ToArray();
            }
            else
                heights = [0, 0, 0, 0];

            return [
                (pos.X + 0 + WorldResources.ModelOffsetX, heights[0], pos.Y + 0 + WorldResources.ModelOffsetZ),
                (pos.X + 1 + WorldResources.ModelOffsetX, heights[1], pos.Y + 0 + WorldResources.ModelOffsetZ),
                (pos.X + 1 + WorldResources.ModelOffsetX, heights[2], pos.Y + 1 + WorldResources.ModelOffsetZ),
                (pos.X + 0 + WorldResources.ModelOffsetX, heights[3], pos.Y + 1 + WorldResources.ModelOffsetZ)
            ];
        }

        public void Reset() {
            Models?.Dispose();
            Models?.Clear();

            Model?.Dispose();
            UntexturedModel?.Dispose();
            SelectionModel?.Dispose();

            Model = null;
            UntexturedModel = null;
            SelectionModel = null;

            Models = null;
        }

        public void Update(IMPD_File mpdFile) {
            Reset();

            var texturesById = mpdFile.TextureCollections != null ? mpdFile.TextureCollections
                .SelectMany(x => x.TextureTable.Rows)
                .GroupBy(x => x.ID)
                .Select(x => x.First())
                .ToDictionary(x => x.ID, x => x.Texture)
                : [];

            var animationsById = mpdFile.TextureAnimations != null ? mpdFile.TextureAnimations.Rows
                .GroupBy(x => x.TextureID)
                .Select(x => x.First())
                .ToDictionary(x => (int) x.TextureID, x => x.Frames.OrderBy(x => x.FrameNum).Select(x => x.Texture).ToArray())
                : [];

            var surfaceQuads           = new List<Quad>();
            var untexturedSurfaceQuads = new List<Quad>();
            var surfaceSelectionQuads  = new List<Quad>();

            Vector3 GetVertexAbnormal(int tileX, int tileY, CornerType corner) {
                var locations = GetBlockLocations(tileX, tileY, corner);
                if (locations.Length == 0 || mpdFile.SurfaceModel?.VertexNormalBlockTable?.Rows == null)
                    return new Vector3(0f, 1 / 32768f, 0f);

                // The vertex abnormals SHOULD be the same, so just use the first one.
                var loc = locations[0];
                return mpdFile.SurfaceModel.VertexNormalBlockTable.Rows[loc.Num][loc.X, loc.Y].ToVector3();
            };

            var textureData = mpdFile.SurfaceModel?.TileTextureRowTable?.Make2DTextureData();
            for (var y = 0; y < WidthInTiles; y++) {
                for (var x = 0; x < HeightInTiles; x++) {
                    TextureAnimation anim = null;
                    byte textureFlags = 0;

                    if (textureData != null) {
                        var key = textureData[x, y];
                        var textureId = textureData[x, y] & 0xFF;
                        textureFlags = (byte) (textureData[x, y] >> 8 & 0xFF);

                        // Get texture. Fetch animated textures if possible.
                        if (textureId != 0xFF && texturesById.ContainsKey(textureId)) {
                            if (animationsById.ContainsKey(textureId))
                                anim = new TextureAnimation(textureId, animationsById[textureId]);
                            else if (texturesById.ContainsKey(textureId))
                                anim = new TextureAnimation(textureId, [texturesById[textureId]]);
                        }
                    }

                    var vertexAbnormals = new Vector3[] {
                        GetVertexAbnormal(x, y, CornerType.TopLeft),
                        GetVertexAbnormal(x, y, CornerType.TopRight),
                        GetVertexAbnormal(x, y, CornerType.BottomRight),
                        GetVertexAbnormal(x, y, CornerType.BottomLeft),
                    };
                    var abnormalVboData = vertexAbnormals.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 3);

                    var vertices = GetTileVertices(mpdFile, new Point(x, y));
                    if (anim != null) {
                        var newQuad = new Quad(vertices, anim, textureFlags);
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec3, "normal", 4, abnormalVboData));
                        surfaceQuads.Add(newQuad);
                    }
                    else {
                        var newQuad = new Quad(vertices);
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec3, "normal", 4, abnormalVboData));
                        untexturedSurfaceQuads.Add(newQuad);
                    }

                    surfaceSelectionQuads.Add(new Quad(vertices, new Vector3(x / (float) WidthInTiles, y / (float) HeightInTiles, 0)));
                    TileDebugText[x, y] =
                        "  [" + (x - 0.5) + ", " + (y - 0.5) + "] Pos: " + vertices[0] + ", Normal: " + vertexAbnormals[0] + "\n" +
                        "  [" + (x + 0.5) + ", " + (y - 0.5) + "] Pos: " + vertices[1] + ", Normal: " + vertexAbnormals[1] + "\n" +
                        "  [" + (x + 0.5) + ", " + (y + 0.5) + "] Pos: " + vertices[2] + ", Normal: " + vertexAbnormals[2] + "\n" +
                        "  [" + (x - 0.5) + ", " + (y + 0.5) + "] Pos: " + vertices[3] + ", Normal: " + vertexAbnormals[3] + "\n";
                }
            }

            var models = new List<QuadModel>();

            if (surfaceQuads.Count > 0) {
                this.Model = new QuadModel(surfaceQuads.ToArray());
                models.Add(this.Model);
            }
            if (untexturedSurfaceQuads.Count > 0) {
                UntexturedModel = new QuadModel(untexturedSurfaceQuads.ToArray());
                models.Add(UntexturedModel);
            }
            if (surfaceSelectionQuads.Count > 0) {
                SelectionModel = new QuadModel(surfaceSelectionQuads.ToArray());
                models.Add(SelectionModel);
            }

            Models = [.. models];
        }

        public QuadModel Model { get; private set; } = null;
        public QuadModel UntexturedModel { get; private set; } = null;
        public QuadModel SelectionModel { get; private set; } = null;

        public DisposableList<QuadModel> Models { get; private set; } = null;

        public string[,] TileDebugText { get; } = new string[64, 64];
    }
}
