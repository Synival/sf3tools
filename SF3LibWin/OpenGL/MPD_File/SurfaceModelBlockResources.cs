﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.Extensions;

namespace SF3.Win.OpenGL.MPD_File {
    public class SurfaceModelBlockResources : IDisposable {
        public SurfaceModelBlockResources(int blockNum) {
            BlockNum = blockNum;
            TileX1 = (blockNum % 16) * 4;
            TileY1 = (blockNum / 16) * 4;
            TileX2 = TileX1 + 4;
            TileY2 = TileY1 + 4;
        }

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

        ~SurfaceModelBlockResources() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine(GetType().Name + ": GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
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

            NeedsUpdate = false;
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

            var terrainTypeTexInfo = Shader.GetTextureInfo(MPD_TextureUnit.TextureTerrainTypes);
            var eventIdTexInfo     = Shader.GetTextureInfo(MPD_TextureUnit.TextureEventIDs);

            var surfaceQuads           = new List<Quad>();
            var untexturedSurfaceQuads = new List<Quad>();
            var surfaceSelectionQuads  = new List<Quad>();

            var textureData = mpdFile.SurfaceModel?.TileTextureRowTable?.Make2DTextureData();
            for (var y = TileY1; y < TileY2; y++) {
                for (var x = TileX1; x < TileX2; x++) {
                    var tile = mpdFile.Tiles[x, y];

                    TextureAnimation anim = null;
                    byte textureFlags = 0;

                    if (textureData != null) {
                        // Get texture. Fetch animated textures if possible.
                        var textureId = tile.ModelTextureID;
                        textureFlags = tile.ModelTextureFlags;

                        if (textureId != 0xFF && texturesById.ContainsKey(textureId)) {
                            if (animationsById.ContainsKey(textureId))
                                anim = new TextureAnimation(textureId, animationsById[textureId]);
                            else if (texturesById.ContainsKey(textureId))
                                anim = new TextureAnimation(textureId, [texturesById[textureId]]);
                        }
                    }

                    var vertexAbnormals = tile.GetVertex3Abnormals();
                    var abnormalVboData = vertexAbnormals.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 3);

                    var terrainType = (int) tile.MoveTerrain;
                    var ttX1 = (terrainType % 4) / 4.0f;
                    var ttY1 = (terrainType / 4) / 4.0f;
                    var ttX2 = ttX1 + 0.25f;
                    var ttY2 = ttY1 + 0.25f;
                    var terrainTypeVboData = new float[4, 2] {
                        { ttX1, ttY2 },
                        { ttX2, ttY2 },
                        { ttX2, ttY1 },
                        { ttX1, ttY1 },
                    };

                    var eventId = (int) tile.EventID;
                    var eidX1 = (eventId % 16) / 16.0f;
                    var eidY1 = (eventId / 16) / 16.0f;
                    var eidX2 = eidX1 + 0.0625f;
                    var eidY2 = eidY1 + 0.0625f;
                    var eventIdVboData = new float[4, 2] {
                        { eidX1, eidY2 },
                        { eidX2, eidY2 },
                        { eidX2, eidY1 },
                        { eidX1, eidY1 },
                    };

                    var vertices = tile.GetSurfaceModelVertices();
                    if (anim != null) {
                        var newQuad = new Quad(vertices, anim, textureFlags);
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec3, "normal", 4, abnormalVboData));
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, terrainTypeTexInfo.TexCoordName, 4, terrainTypeVboData));
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, eventIdTexInfo.TexCoordName, 4, eventIdVboData));
                        surfaceQuads.Add(newQuad);
                    }
                    else {
                        var newQuad = new Quad(vertices);
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec3, "normal", 4, abnormalVboData));
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, terrainTypeTexInfo.TexCoordName, 4, terrainTypeVboData));
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, eventIdTexInfo.TexCoordName, 4, eventIdVboData));
                        untexturedSurfaceQuads.Add(newQuad);
                    }

                    var selectionColor = new Vector3(x / (float) SurfaceModelResources.WidthInTiles, y / (float) SurfaceModelResources.HeightInTiles, 0);
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

            Models = [.. models];
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

        public DisposableList<QuadModel> Models { get; private set; } = null;

        public bool NeedsUpdate { get; private set; } = true;
    }
}
