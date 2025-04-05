using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CommonLib.Types;
using OpenTK.Graphics.OpenGL;
using SF3.Types;
using SF3.Win.OpenGL.MPD_File;
using SF3.Win.Types;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl {
        private void InitEditing() {
            MouseDown += (s, e) => OnMouseDownEditing(e);
            MouseUp   += (s, e) => OnMouseUpEditing(e);
            MouseMove += (s, e) => OnMouseMoveEditing(e);
        }

        private void OnMouseDownEditing(MouseEventArgs e) {
            var cursorMode = CursorMode;

            if (cursorMode == ViewerCursorMode.Select && e.Button == MouseButtons.Left)
                SelectTile(_tileHoverPos);
            if (cursorMode.IsDrawingMode()) {
                DrawTileAtCursor();
                _lastTileEdited = _tileHoverPos;
            }
        }

        private void OnMouseUpEditing(MouseEventArgs e) =>
            _lastTileEdited = null;

        private void OnMouseMoveEditing(MouseEventArgs e) {
            var cursorMode = CursorMode;
            if (_mouseButtons == MouseButtons.Left && CursorMode.IsDrawingMode() && _lastTileEdited != _tileHoverPos) {
                DrawTileAtCursor();
                _lastTileEdited = _tileHoverPos;
            }
        }

        public void SelectTile(Point? tilePos) {
            if (_tileSelectedPos == tilePos)
                return;

            _tileSelectedPos = tilePos;
            var tile = (_tileSelectedPos == null) ? null : MPD_File.Tiles[_tileSelectedPos.Value.X, _tileSelectedPos.Value.Y];
            TilePropertiesControl.Tile = tile;
            _tileSelectedNeedsUpdate = true;

            Invalidate();
        }

        private void UpdateTilePosition() {
            // Don't allow changing tiles while the mouse is down.
            if (!(_mouseButtons == 0 || CursorMode.IsDrawingMode()))
                return;

            if (_mousePos == null) {
                UpdateTilePosition(null);
                return;
            }

            var pixel = new byte[3];
            using (_selectFramebuffer.Use())
                GL.ReadPixels(_mousePos.Value.X, Height - _mousePos.Value.Y - 1, 1, 1, PixelFormat.Rgb, PixelType.UnsignedByte, pixel);

            if (pixel[2] == 255)
                UpdateTilePosition(null);
            else {
                UpdateTilePosition(new Point(
                    (int) Math.Round(pixel[0] / (255.0f / SurfaceModelResources.WidthInTiles)),
                    (int) Math.Round(pixel[1] / (255.0f / SurfaceModelResources.HeightInTiles))
                ));
            }
        }

        private void UpdateTilePosition(Point? pos) {
            // All invalid tile values should be 'null'.
            if (pos.HasValue && (pos.Value.X < 0 || pos.Value.Y < 0 || pos.Value.X > 63 || pos.Value.Y > 63))
                pos = null;

            // Early exit if no change is necessary.
            if (_tileHoverPos == pos)
                return;

            _tileHoverPos = pos;
            _surfaceEditor.UpdateTileHoverModel(MPD_File, _general, _tileHoverPos);

            Invalidate();
        }

        private void DrawTileAtCursor() {
            if (_tileHoverPos == null)
                return;
            DrawTileAt(_tileHoverPos.Value.X, _tileHoverPos.Value.Y);
        }

        private void DrawTileAt(int x, int y) {
            // TODO:
            // ===========================================
            // ALL OF THIS IS BAD!!!
            // ===========================================
            // It's hard-coded to FIELD.MPD on the premium disk.
            // It's hacked together quickly.
            // The math is bogus.
            // It's probably really buggy.
            // Let's do it right sometime soon, shall we?

            if (MPD_File == null || MPD_File.SurfaceModel == null || x < 0 || y < 0 || x >= 64 || y >= 64)
                return;

            const byte texGrassland     = 0x01;
            const byte texDirt          = 0x29;
            const byte texDarkGrass     = 0x1C;
            const byte texBrownMountain = 0x42;
            const byte texGreyMountain  = 0x4C;
            const byte texMountainPeak  = 0x4E;
            const byte texDesert        = 0x7C;
            const byte texRiver         = 0x2A;
            const byte texBridge        = 0x74;
            const byte texWater         = 0xFF;

            byte GetTextureForCursorMode() {
                switch (CursorMode) {
                    case ViewerCursorMode.DrawGrassland:     return texGrassland;
                    case ViewerCursorMode.DrawDirt:          return texDirt;
                    case ViewerCursorMode.DrawDarkGrass:     return texDarkGrass;
                    case ViewerCursorMode.DrawForest:        return texDarkGrass;
                    case ViewerCursorMode.DrawBrownMountain: return texBrownMountain;
                    case ViewerCursorMode.DrawGreyMountain:  return texGreyMountain;
                    case ViewerCursorMode.DrawMountainPeak:  return texMountainPeak;
                    case ViewerCursorMode.DrawDesert:        return texDesert;
                    case ViewerCursorMode.DrawRiver:         return texRiver;
                    case ViewerCursorMode.DrawBridge:        return texBridge;
                    case ViewerCursorMode.DrawWater:         return texWater;
                    default:                                 return 0xFF;
                }
            }

            TerrainType GetTerrainTypeForCursorMode() {
                switch (CursorMode) {
                    case ViewerCursorMode.DrawGrassland:     return TerrainType.Grassland;
                    case ViewerCursorMode.DrawDirt:          return TerrainType.Dirt;
                    case ViewerCursorMode.DrawDarkGrass:     return TerrainType.DarkGrass;
                    case ViewerCursorMode.DrawForest:        return TerrainType.Forest;
                    case ViewerCursorMode.DrawBrownMountain: return TerrainType.BrownMountain;
                    case ViewerCursorMode.DrawGreyMountain:  return TerrainType.GreyMountain;
                    case ViewerCursorMode.DrawMountainPeak:  return TerrainType.NoEntry;
                    case ViewerCursorMode.DrawDesert:        return TerrainType.Desert;
                    case ViewerCursorMode.DrawRiver:         return TerrainType.Water;
                    case ViewerCursorMode.DrawBridge:        return TerrainType.Dirt;
                    case ViewerCursorMode.DrawWater:         return TerrainType.Water;
                    default:                                 return TerrainType.NoEntry;
                }
            }

            byte[] GetLayersForTexture(byte texId) {
                switch (texId) {
                    case texGrassland:     return [texWater, texGrassland];
                    case texDirt:          return [texWater, texGrassland, texDirt];
                    case texDarkGrass:     return [texWater, texGrassland, texDarkGrass];
                    case texBrownMountain: return [texWater, texGrassland, texDarkGrass, texBrownMountain];
                    case texGreyMountain:  return [texWater, texGrassland, texDarkGrass, texGreyMountain];
                    case texMountainPeak:  return [texWater, texGrassland, texDarkGrass, texGreyMountain, texMountainPeak];
                    case texDesert:        return [texWater, texGrassland, texDesert];
                    case texRiver:         return [texWater, texRiver];
                    case texBridge:        return [texWater, texRiver];
                    case texWater:         return [texWater];
                    default:               return [texWater];
                }
            }

            const int nearbyRange = 1;

            Random random = null;
            float GetRandomRange(float min, float max)
                => ((float) random.NextDouble() * (max - min)) + min;

            float GetHeightBonusForTexture(byte texId, int atVertexCount, int nearbyCount) {
                switch (texId) {
                    case texGrassland:     return GetRandomRange(0.0625f, 0.1875f) + ((nearbyCount - 1) * 0.025f);
                    case texDirt:          return 0f;
                    case texDarkGrass:     return atVertexCount * 0.03125f;
                    case texBrownMountain: return atVertexCount == 4f ? 0.75f : 0f;
                    case texGreyMountain:  return atVertexCount == 4f ? GetRandomRange(0.5f, 0.75f) + GetRandomRange(0.75f, 1.25f) * ((nearbyCount - 4) * 0.0625f) : 0f;
                    case texMountainPeak:  return atVertexCount == 4f ? 0.75f + ((nearbyCount - 4) * 0.125f) : 0f;
                    case texDesert:        return (nearbyCount - 1) * -0.025f;
                    case texRiver:         return GetRandomRange(0.00f, 0.0625f) + (atVertexCount - 1) * 0.05f;
                    case texBridge:        return 0.0625f;
                    case texWater:         return 6.25f;
                    default:               return 6.25f;
                }
            }

            var texID       = GetTextureForCursorMode();
            var terrainType = GetTerrainTypeForCursorMode();
            var layers      = GetLayersForTexture(texID);

            var thisTile = MPD_File.Tiles[x, y];
            thisTile.ModelTextureID  = texID;
            thisTile.MoveTerrainType = terrainType;
            var updateSurfaceModel = !thisTile.ModelIsFlat;

            int SeedForCoordinates(int sx, int sy) {
                var seed = 2166136261;
                seed += (uint) sx;
                seed *= 16777619;
                seed += (uint) sy;
                seed *= 16777619;
                return (int) seed;
            }

            for (int x2 = Math.Max(0, x - nearbyRange); x2 <= Math.Min(63, x + nearbyRange); x2++) {
                for (int y2 = Math.Max(0, y - nearbyRange); y2 <= Math.Min(63, y + nearbyRange); y2++) {
                    var affectedTile = MPD_File.Tiles[x2, y2];
                    foreach (var corner in Enum.GetValues<CornerType>()) {
                        var vx = x2 + corner.GetVertexOffsetX();
                        var vy = y2 + corner.GetVertexOffsetY();

                        Dictionary<byte, int> layersAtVertex = [];
                        for (int tx = Math.Max(0, vx - 1); tx <= Math.Min(63, vx); tx++) {
                            for (int ty = Math.Max(0, vy - 1); ty <= Math.Min(63, vy); ty++) {
                                random = new Random(SeedForCoordinates(tx, ty));

                                var neighborTexID = MPD_File.Tiles[tx, ty].ModelTextureID;
                                var neighborLayers = GetLayersForTexture(neighborTexID);
                                foreach (var layer in neighborLayers)
                                    layersAtVertex[layer] = (layersAtVertex.TryGetValue(layer, out var v) ? v : 0) + 1;
                            }
                        }

                        Dictionary<byte, int> layersNearby = [];
                        for (int tx = Math.Max(0, vx - 1 - nearbyRange); tx <= Math.Min(63, vx + nearbyRange); tx++) {
                            for (int ty = Math.Max(0, vy - 1 - nearbyRange); ty <= Math.Min(63, vy + nearbyRange); ty++) {
                                var neighborTexID = MPD_File.Tiles[tx, ty].ModelTextureID;
                                var neighborLayers = GetLayersForTexture(neighborTexID);
                                foreach (var layer in neighborLayers)
                                    layersNearby[layer] = (layersNearby.TryGetValue(layer, out var v) ? v : 0) + 1;
                            }
                        }

                        var vertexHeight = 0.00f;
                        foreach (var kv in layersAtVertex)
                            vertexHeight += GetHeightBonusForTexture(kv.Key, kv.Value, layersNearby[kv.Key]);

                        affectedTile.SetMoveHeightmap(corner, vertexHeight);
                        affectedTile.CopyMoveHeightToNonFlatNeighbors(corner);
                        if (updateSurfaceModel)
                            affectedTile.SetModelVertexHeightmap(corner, vertexHeight);
                    }
                }
            }

            for (int tx = x - 2 - nearbyRange; tx <= x + 2 + nearbyRange; tx++) {
                for (int ty = y - 2 - nearbyRange; ty <= y + 2 + nearbyRange; ty++) {
                    if (tx >= 0 && ty >= 0 && tx < 64 && ty < 64) {
                        var affectedTile = MPD_File.Tiles[tx, ty];
                        affectedTile.MoveHeight = affectedTile.GetAverageHeight();
                        affectedTile.UpdateNormals();
                    }
                }
            }

            bool modelsChanged = false;
            if (thisTile.MoveTerrainType == TerrainType.Forest)
                modelsChanged = thisTile.AdoptTree();
            else
                modelsChanged = thisTile.OrphanTree();
            if (modelsChanged)
                OnModelsUpdated(this, EventArgs.Empty);

            Invalidate();
        }

        private Point? _tileHoverPos = null;
        private Point? _tileSelectedPos = null;
        private Point? _lastTileEdited = null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TilePropertiesControl TilePropertiesControl { get; set; } = null;
    }
}
