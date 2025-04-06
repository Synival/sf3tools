using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CommonLib.Types;
using OpenTK.Graphics.OpenGL;
using SF3.FieldEditing;
using SF3.Types;
using SF3.Win.OpenGL.MPD_File;
using SF3.Win.Types;
using static SF3.FieldEditing.Constants;

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

            TileType GetTileTypeForCursorMode() {
                switch (CursorMode) {
                    case ViewerCursorMode.DrawGrassland:     return TileType.Grass;
                    case ViewerCursorMode.DrawDirt:          return TileType.Dirt;
                    case ViewerCursorMode.DrawDarkGrass:     return TileType.DarkGrass;
                    case ViewerCursorMode.DrawForest:        return TileType.DarkGrass;
                    case ViewerCursorMode.DrawBrownMountain: return TileType.Hill;
                    case ViewerCursorMode.DrawGreyMountain:  return TileType.Mountain;
                    case ViewerCursorMode.DrawMountainPeak:  return TileType.Peak;
                    case ViewerCursorMode.DrawDesert:        return TileType.Desert;
                    case ViewerCursorMode.DrawRiver:         return TileType.River;
                    case ViewerCursorMode.DrawBridge:        return TileType.Bridge;
                    case ViewerCursorMode.DrawWater:         return TileType.Water;
                    default:                                 return TileType.Water;
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

            TileType[] GetLayersForTileType(TileType tileType) {
                switch (tileType) {
                    case TileType.Grass:     return [TileType.Water, TileType.Grass];
                    case TileType.Dirt:      return [TileType.Water, TileType.Grass, TileType.Dirt];
                    case TileType.DarkGrass: return [TileType.Water, TileType.Grass, TileType.DarkGrass];
                    case TileType.Hill:      return [TileType.Water, TileType.Grass, TileType.DarkGrass, TileType.Hill];
                    case TileType.Mountain:  return [TileType.Water, TileType.Grass, TileType.DarkGrass, TileType.Mountain];
                    case TileType.Peak:      return [TileType.Water, TileType.Grass, TileType.DarkGrass, TileType.Mountain, TileType.Peak];
                    case TileType.Desert:    return [TileType.Water, TileType.Grass, TileType.Desert];
                    case TileType.River:     return [TileType.Water, TileType.River];
                    case TileType.Bridge:    return [TileType.Water, TileType.River];
                    case TileType.Water:     return [TileType.Water];
                    default:                 return [TileType.Water];
                }
            }

            const int nearbyRange = 1;

            Random random = null;
            float GetRandomRange(float min, float max)
                => ((float) random.NextDouble() * (max - min)) + min;

            float GetHeightBonusForTileType(TileType tileType, int atVertexCount, int nearbyCount) {
                switch (tileType) {
                    case TileType.Grass:     return GetRandomRange(0.0625f, 0.1875f) + ((nearbyCount - 1) * 0.025f);
                    case TileType.Dirt:      return 0f;
                    case TileType.DarkGrass: return atVertexCount * 0.03125f;
                    case TileType.Hill:      return atVertexCount == 4f ? 0.75f : 0f;
                    case TileType.Mountain:  return atVertexCount == 4f ? GetRandomRange(0.5f, 0.75f) + GetRandomRange(0.75f, 1.25f) * ((nearbyCount - 4) * 0.0625f) : 0f;
                    case TileType.Peak:      return atVertexCount == 4f ? 0.75f + ((nearbyCount - 4) * 0.125f) : 0f;
                    case TileType.Desert:    return (nearbyCount - 1) * -0.025f;
                    case TileType.River:     return GetRandomRange(0.00f, 0.0625f) + (atVertexCount - 1) * 0.05f;
                    case TileType.Bridge:    return 0.0625f;
                    case TileType.Water:     return 6.25f;
                    default:                 return 6.25f;
                }
            }

            var tileType    = GetTileTypeForCursorMode();
            var terrainType = GetTerrainTypeForCursorMode();
            var layers      = GetLayersForTileType(tileType);

            var thisTile = MPD_File.Tiles[x, y];
            thisTile.ModelTextureID  = GetDefaultTexIdByTileType(tileType);
            thisTile.MoveTerrainType = terrainType;
            var updateSurfaceModel = !thisTile.ModelIsFlat;

            for (int x2 = Math.Max(0, x - nearbyRange); x2 <= Math.Min(63, x + nearbyRange); x2++) {
                for (int y2 = Math.Max(0, y - nearbyRange); y2 <= Math.Min(63, y + nearbyRange); y2++) {
                    var affectedTile = MPD_File.Tiles[x2, y2];
                    foreach (var corner in Enum.GetValues<CornerType>()) {
                        var vx = x2 + corner.GetVertexOffsetX();
                        var vy = y2 + corner.GetVertexOffsetY();

                        Dictionary<TileType, int> layersAtVertex = [];
                        for (int tx = Math.Max(0, vx - 1); tx <= Math.Min(63, vx); tx++) {
                            for (int ty = Math.Max(0, vy - 1); ty <= Math.Min(63, vy); ty++) {
                                var neighborTile = MPD_File.Tiles[tx, ty];
                                random = new Random(neighborTile.RandomSeed);

                                var neighborTexId = neighborTile.ModelTextureID;
                                var neighborTileType = GetTileTypeByTexID(neighborTexId) ?? TileType.Water;
                                var neighborLayers = GetLayersForTileType(neighborTileType);
                                foreach (var layer in neighborLayers)
                                    layersAtVertex[layer] = (layersAtVertex.TryGetValue(layer, out var v) ? v : 0) + 1;
                            }
                        }

                        Dictionary<TileType, int> layersNearby = [];
                        for (int tx = Math.Max(0, vx - 1 - nearbyRange); tx <= Math.Min(63, vx + nearbyRange); tx++) {
                            for (int ty = Math.Max(0, vy - 1 - nearbyRange); ty <= Math.Min(63, vy + nearbyRange); ty++) {
                                var neighborTexId = MPD_File.Tiles[tx, ty].ModelTextureID;
                                var neighborTileType = GetTileTypeByTexID(neighborTexId) ?? TileType.Water;
                                var neighborLayers = GetLayersForTileType(neighborTileType);
                                foreach (var layer in neighborLayers)
                                    layersNearby[layer] = (layersNearby.TryGetValue(layer, out var v) ? v : 0) + 1;
                            }
                        }

                        var vertexHeight = 0.00f;
                        foreach (var kv in layersAtVertex)
                            vertexHeight += GetHeightBonusForTileType(kv.Key, kv.Value, layersNearby[kv.Key]);

                        affectedTile.SetMoveHeightmap(corner, vertexHeight);
                        affectedTile.CopyMoveHeightToNonFlatNeighbors(corner);
                        if (updateSurfaceModel)
                            affectedTile.SetModelVertexHeightmap(corner, vertexHeight);
                    }
                }
            }

            for (int tx = x - 1; tx <= x + 1; tx++)
                for (int ty = y - 1; ty <= y + 1; ty++)
                    if (tx >= 0 && ty >= 0 && tx < 64 && ty < 64)
                        FieldEditing.FieldEditing.UpdateTileTexture(MPD_File.Tiles[tx, ty], true);

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
