using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using CommonLib.Types;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Types;
using SF3.Win.Extensions;
using static CommonLib.Types.CornerTypeConsts;

namespace SF3.Win.OpenGL.MPD_File {
    public class SurfaceModelBlockResources : ResourcesBase, IMPD_Resources {
        public SurfaceModelBlockResources(int blockNum) {
            BlockNum = blockNum;
            TileX1 = (blockNum % 16) * 4;
            TileY1 = (blockNum / 16) * 4;
            TileX2 = TileX1 + 4;
            TileY2 = TileY1 + 4;
        }

        protected override void PerformInit() { }
        public override void DeInit() { }

        public override void Reset() {
            Models?.Dispose();
            Models?.Clear();

            Model?.Dispose();
            UntexturedModel?.Dispose();
            SelectionModel?.Dispose();

            Model = null;
            UntexturedModel = null;
            SelectionModel = null;

            NeedsUpdate = false;
        }

        private readonly float[,] _applyLightingVboData = new float[,] {{1}, {1}, {1}, {1}};

        public void Update(IMPD_File mpdFile) {
            Reset();

            var texturesById = mpdFile.TextureCollections != null ? mpdFile.TextureCollections
                .Where(x => x?.TextureTable != null && x.TextureTable.Collection == TextureCollectionType.PrimaryTextures)
                .SelectMany(x => x.TextureTable)
                .GroupBy(x => x.ID)
                .Select(x => x.First())
                .ToDictionary(x => x.ID, x => x.Texture)
                : [];

            var animationsById = mpdFile.TextureAnimations != null ? mpdFile.TextureAnimations
                .GroupBy(x => x.TextureID)
                .Select(x => x.First())
                .ToDictionary(x => (int) x.TextureID, x => new { Textures = x.FrameTable.OrderBy(x => x.FrameNum).Select(x => x.Texture).ToArray(), x.FrameTimerStart })
                : [];

            var terrainTypeTexInfo = Shader.GetTextureInfo(MPD_TextureUnit.TextureTerrainTypes);
            var eventIdTexInfo     = Shader.GetTextureInfo(MPD_TextureUnit.TextureEventIDs);

            var surfaceQuads           = new List<Quad>();
            var untexturedSurfaceQuads = new List<Quad>();
            var surfaceSelectionQuads  = new List<Quad>();

            var textureData = mpdFile.SurfaceModel?.TileTextureRowTable?.Make2DTextureData();
            for (var y = TileY1; y < TileY2; y++) {
                for (var x = TileX1; x < TileX2; x++) {
                    var tile = mpdFile.Tiles[x, y];

                    TextureAnimation  anim   = null;
                    TextureRotateType rotate = TextureRotateType.NoRotation;
                    TextureFlipType   flip   = TextureFlipType.NoFlip;

                    if (textureData != null) {
                        // Get texture. Fetch animated textures if possible.
                        var textureId = tile.ModelTextureID;
                        rotate = tile.ModelTextureRotate;
                        flip = tile.ModelTextureFlip;

                        if (textureId != 0xFF && texturesById.ContainsKey(textureId)) {
                            if (animationsById.ContainsKey(textureId))
                                anim = new TextureAnimation(textureId, animationsById[textureId].Textures, animationsById[textureId].FrameTimerStart);
                            else if (texturesById.ContainsKey(textureId))
                                anim = new TextureAnimation(textureId, [texturesById[textureId]], 0);
                        }
                    }

                    var vertexNormals = tile.GetVertex3Normals();

                    // Convert normals to normals understood by the shader (which doesn't make much sense, but it works...)
                    var normalVboData = vertexNormals.SelectMany(x => (x * new Vector3(1, -1, -1)).ToFloatArray()).ToArray().To2DArray(4, 3);

                    // This simulates a bug where normal vector X and Z components >= 0.50 or < -0.50 flip sides, likely
                    // due to a mistake involving the strange compressed FIXED binary format.
                    for (var i = 0; i < normalVboData.GetLength(0); i++) {
                        for (var j = 0; j < normalVboData.GetLength(1); j++) {
                            if (j == 1)
                                continue;
                            if (normalVboData[i, j] > 0.50f)
                                normalVboData[i, j] -= 1.00f;
                            else if (normalVboData[i, j] <= -0.50f)
                                normalVboData[i, j] += 1.00f;
                        }
                    }

                    var terrainType = (int) tile.MoveTerrainType;
                    var ttX = (terrainType % 4) / 4.0f;
                    var ttY = (terrainType / 4) / 4.0f;
                    const float ttWidth = 0.25f;
                    const float ttHeight = 0.25f;

                    var terrainTypeData = new Vector2[4];
                    terrainTypeData[(int) CornerType.TopRight]    = new Vector2(ttX + Corner1UVX * ttWidth, ttY + Corner1UVY * ttHeight);
                    terrainTypeData[(int) CornerType.TopLeft]     = new Vector2(ttX + Corner2UVX * ttWidth, ttY + Corner2UVY * ttHeight);
                    terrainTypeData[(int) CornerType.BottomLeft]  = new Vector2(ttX + Corner3UVX * ttWidth, ttY + Corner3UVY * ttHeight);
                    terrainTypeData[(int) CornerType.BottomRight] = new Vector2(ttX + Corner4UVX * ttWidth, ttY + Corner4UVY * ttHeight);
                    var terrainTypeVboData = terrainTypeData.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 2);

                    var eventId = (int) tile.EventID;
                    var eidX = (eventId % 16) / 16.0f;
                    var eidY = (eventId / 16) / 16.0f;
                    const float eidWidth = 0.0625f;
                    const float eidHeight = 0.0625f;

                    var eventIdData = new Vector2[4];
                    eventIdData[(int) CornerType.TopRight]    = new Vector2(eidX + Corner1UVX * eidWidth, eidY + Corner1UVY * eidHeight);
                    eventIdData[(int) CornerType.TopLeft]     = new Vector2(eidX + Corner2UVX * eidWidth, eidY + Corner2UVY * eidHeight);
                    eventIdData[(int) CornerType.BottomLeft]  = new Vector2(eidX + Corner3UVX * eidWidth, eidY + Corner3UVY * eidHeight);
                    eventIdData[(int) CornerType.BottomRight] = new Vector2(eidX + Corner4UVX * eidWidth, eidY + Corner4UVY * eidHeight);
                    var eventIdVboData = eventIdData.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 2);

                    var vertices = tile.GetSurfaceModelVertices();
                    if (anim != null) {
                        var newQuad = new Quad(vertices, anim, rotate, flip);
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec3, "normal", 4, normalVboData));
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, terrainTypeTexInfo.TexCoordName, 4, terrainTypeVboData));
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, eventIdTexInfo.TexCoordName, 4, eventIdVboData));
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.Float, "applyLighting", 4, _applyLightingVboData));
                        surfaceQuads.Add(newQuad);
                    }
                    else {
                        var newQuad = new Quad(vertices);
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec3, "normal", 4, normalVboData));
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, terrainTypeTexInfo.TexCoordName, 4, terrainTypeVboData));
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, eventIdTexInfo.TexCoordName, 4, eventIdVboData));
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.Float, "applyLighting", 4, _applyLightingVboData));
                        untexturedSurfaceQuads.Add(newQuad);
                    }

                    var selectionColor = new Vector4(x / (float) SurfaceModelResources.WidthInTiles, y / (float) SurfaceModelResources.HeightInTiles, 0, 1);
                    surfaceSelectionQuads.Add(new Quad(vertices, selectionColor));
                }
            }

            var models = new List<QuadModel>();

            if (surfaceQuads.Count > 0) {
                Model = new QuadModel(surfaceQuads.ToArray());
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

            Models.AddRange(models);
            NeedsUpdate = false;
        }

        public void Invalidate()
            => NeedsUpdate = true;

        public int BlockNum { get; }
        public int TileX1 { get; }
        public int TileY1 { get; }
        public int TileX2 { get; }
        public int TileY2 { get; }

        public QuadModel Model { get; private set; } = null;
        public QuadModel UntexturedModel { get; private set; } = null;
        public QuadModel SelectionModel { get; private set; } = null;

        public DisposableList<QuadModel> Models { get; } = [];

        public bool NeedsUpdate { get; private set; } = true;
    }
}
