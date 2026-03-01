using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Types;
using OpenTK.Graphics.OpenGL;
using SF3.Actors;
using SF3.FieldEditing;
using SF3.Models.Files.MPD;
using SF3.MPD.Interfaces;
using SF3.Types;
using SF3.Win.App;
using SF3.Win.Types;
using static SF3.FieldEditing.Constants;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl {
        public interface ISelectableObject {}

        public class SelectableTile(int x, int y) : ISelectableObject {
            public int X = x;
            public int Y = y;
        }

        public class SelectableModel(MPD_CollectionType collection, int instanceId) : ISelectableObject {
            public MPD_CollectionType Collection = collection;
            public int InstanceID = instanceId;
        }

        public class SelectableActor(int id) : ISelectableObject {
            public int ID = id;
        }

        private void InitEditing() {
            MouseDown += (s, e) => OnMouseDownEditing(e);
            MouseUp   += (s, e) => OnMouseUpEditing(e);
            MouseMove += (s, e) => OnMouseMoveEditing(e);
        }

        private void OnMouseDownEditing(MouseEventArgs e) {
            if (e.Button != MouseButtons.Left)
                return;

            var cursorMode = CursorMode;
            if (cursorMode == ViewerCursorMode.Select)
                SelectObject(_mouseoverObject);
            else if (_mouseoverObject is SelectableTile tile && cursorMode.IsDrawingMode()) {
                DrawMouseoverTile();
                _lastMouseoverTileEdited = tile;
            }
        }

        private void OnMouseUpEditing(MouseEventArgs e) =>
            _lastMouseoverTileEdited = null;

        private void OnMouseMoveEditing(MouseEventArgs e) {
            var cursorMode = CursorMode;
            if (_mouseButtons == MouseButtons.Left && CursorMode.IsDrawingMode() && _lastMouseoverTileEdited != _mouseoverObject && _mouseoverObject is SelectableTile tile) {
                DrawMouseoverTile();
                _lastMouseoverTileEdited = tile;
            }
        }

        public void SelectObject(ISelectableObject obj) {
            if ((_selectedObjects.Count == 1 && _selectedObjects[0] == obj) || (_selectedObjects.Count == 0 && obj == null))
                return;

            // TODO: This logic is very wrong for multiple selection.
            var currentSelectedObject = (_selectedObjects.Count == 1) ? _selectedObjects[0] : null;

            var oldTile = currentSelectedObject as SelectableTile;
            var newTile = obj as SelectableTile;

            var oldModel = currentSelectedObject as SelectableModel;
            var newModel = obj as SelectableModel;

            var oldActor = currentSelectedObject as SelectableActor;
            var newActor = obj as SelectableActor;

            object newEventObject = null;

            _selectedObjects.Clear();
            if (obj != null)
                _selectedObjects.Add(obj);

            if (oldTile != newTile) {
                var surfaceTile = (newTile == null) ? null : MPD_File.Surface.GetTile(newTile.X, newTile.Y);
                if (surfaceTile != null)
                    newEventObject = surfaceTile;
            }

            if (oldModel != newModel) {
                IMPD_ModelInstance modelInstance = null;
                if (newModel != null) {
                    var collection = (MPD_File.ModelCollections?.TryGetValue(newModel.Collection, out var collectionObj) == true) ? collectionObj : null;
                    modelInstance = collection?.ModelInstances?.FirstOrDefault(x => x.ID == newModel.InstanceID);
                }
                if (modelInstance != null)
                    newEventObject = modelInstance;
            }

            if (oldActor != newActor) {
                IActor actor = null;
                if (newActor != null) {
                    var actors = AppScene.Get().ActiveActorCollection?.Actors;
                    if (actors != null)
                        actor = actors.FirstOrDefault(x => x.ID == newActor.ID);
                }
                if (actor != null)
                    newEventObject = actor;
            }

            if (_objectSelectedEventObject != newEventObject) {
                ObjectSelected?.Invoke(this, newEventObject);
                _objectSelectedEventObject = newEventObject;
            }

            InvalidateEditor();
        }

        private void UpdateMouseoverObject() {
            // Don't allow changing tiles while the mouse is down.
            if (!(_mouseButtons == 0 || CursorMode.IsDrawingMode()))
                return;

            if (_mousePos == null) {
                UpdateMouseoverObject(null);
                return;
            }

            var pixel = new byte[3];
            using (_selectFramebuffer.UseRead()) {
                GL.ReadPixels(_mousePos.Value.X, Height - _mousePos.Value.Y - 1, 1, 1, PixelFormat.Rgb, PixelType.UnsignedByte, pixel);
                for (int i = 0; i < 3; i++)
                    pixel[i] = (byte) Math.Round(pixel[i] / (255f / 64f));
            }

            if (pixel[2] == 0)
                UpdateMouseoverObject(new SelectableTile(pixel[0], pixel[1]));
            else if (pixel[2] == 1)
                UpdateMouseoverObject(new SelectableModel(MPD_CollectionType.Primary, pixel[0] + pixel[1] * 64));
            else if (pixel[2] == 2)
                UpdateMouseoverObject(new SelectableModel(MPD_CollectionType.ExtraModels, pixel[0] + pixel[1] * 64));
            else if (pixel[2] == 3)
                UpdateMouseoverObject(new SelectableActor(pixel[0] + pixel[1] * 64));
            else
                UpdateMouseoverObject(null);
        }

        private void UpdateMouseoverObject(ISelectableObject obj) {
            if (obj is SelectableTile tile) {
                // All invalid tile values should be 'null'.
                if (tile.X < 0 || tile.Y < 0 || tile.X > 63 || tile.Y > 63)
                    obj = null;
            }

            // Early exit if no change is necessary.
            if (_mouseoverObject == obj)
                return;

            _mouseoverObject = obj;
            InvalidateEditor();
        }

        private void DrawMouseoverTile() {
            var tile = _mouseoverObject as SelectableTile;
            if (tile == null)
                return;
            DrawTileAt(tile.X, tile.Y);
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

            if (MPD_File?.Surface?.HasModel != true || x < 0 || y < 0 || x >= 64 || y >= 64)
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

            int GetHeightBonusForTileType(TileType tileType, int atVertexCount, int nearbyCount) {
                float GetFloatValue() {
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
                return (int) (GetFloatValue() * 16);
            }

            var tileType    = GetTileTypeForCursorMode();
            var terrainType = GetTerrainTypeForCursorMode();
            var layers      = GetLayersForTileType(tileType);

            var thisTile = MPD_File.Surface.GetTile(x, y);
            thisTile.TextureID  = GetDefaultTexIdByTileType(tileType);
            thisTile.TerrainType = terrainType;
            var updateSurfaceModel = !thisTile.IsFlat;

            MPD_File.Surface.NormalSettings = _appState.MakeNormalCalculationSettings();
            for (int x2 = Math.Max(0, x - nearbyRange); x2 <= Math.Min(63, x + nearbyRange); x2++) {
                for (int y2 = Math.Max(0, y - nearbyRange); y2 <= Math.Min(63, y + nearbyRange); y2++) {
                    var affectedTile = MPD_File.Surface.GetTile(x2, y2);
                    foreach (var corner in Enum.GetValues<CornerType>()) {
                        var vx = x2 + corner.GetVertexOffsetX();
                        var vy = y2 + corner.GetVertexOffsetY();

                        Dictionary<TileType, int> layersAtVertex = [];
                        for (int tx = Math.Max(0, vx - 1); tx <= Math.Min(63, vx); tx++) {
                            for (int ty = Math.Max(0, vy - 1); ty <= Math.Min(63, vy); ty++) {
                                var neighborTile = MPD_File.Surface.GetTile(tx, ty);
                                random = new Random(neighborTile.RandomSeed);

                                var neighborTexId = neighborTile.TextureID;
                                var neighborTileType = GetTileTypeByTexID(neighborTexId) ?? TileType.Water;
                                var neighborLayers = GetLayersForTileType(neighborTileType);
                                foreach (var layer in neighborLayers)
                                    layersAtVertex[layer] = (layersAtVertex.TryGetValue(layer, out var v) ? v : 0) + 1;
                            }
                        }

                        Dictionary<TileType, int> layersNearby = [];
                        for (int tx = Math.Max(0, vx - 1 - nearbyRange); tx <= Math.Min(63, vx + nearbyRange); tx++) {
                            for (int ty = Math.Max(0, vy - 1 - nearbyRange); ty <= Math.Min(63, vy + nearbyRange); ty++) {
                                var neighborTexId = MPD_File.Surface.GetTile(tx, ty).TextureID;
                                var neighborTileType = GetTileTypeByTexID(neighborTexId) ?? TileType.Water;
                                var neighborLayers = GetLayersForTileType(neighborTileType);
                                foreach (var layer in neighborLayers)
                                    layersNearby[layer] = (layersNearby.TryGetValue(layer, out var v) ? v : 0) + 1;
                            }
                        }

                        int vertexHeight = 0;
                        foreach (var kv in layersAtVertex)
                            vertexHeight += GetHeightBonusForTileType(kv.Key, kv.Value, layersNearby[kv.Key]);
                        affectedTile.SetVertexHeight(corner, (byte) Math.Clamp(vertexHeight, 0, 255));
                    }
                }
            }

            for (int tx = x - 1; tx <= x + 1; tx++)
                for (int ty = y - 1; ty <= y + 1; ty++)
                    if (tx >= 0 && ty >= 0 && tx < 64 && ty < 64)
                        FieldEditing.FieldEditing.UpdateTileTexture(MPD_File.Surface.GetTile(tx, ty), true);

            if (thisTile is SurfaceTile fileTile) {
                bool modelsChanged = false;
                if (thisTile.TerrainType == TerrainType.Forest)
                    modelsChanged = fileTile.AdoptTree();
                else
                    modelsChanged = fileTile.OrphanTree();
                if (modelsChanged)
                    OnModelsUpdated(this, EventArgs.Empty);
            }

            InvalidateFrame();
        }

        private ISelectableObject _mouseoverObject = null;
        private List<ISelectableObject> _selectedObjects = new List<ISelectableObject>();

        private SelectableTile _lastMouseoverTileEdited = null;

        private object _objectSelectedEventObject = null;

        public delegate void ObjectSelectedEventHandler(object sender, object obj);
        public event ObjectSelectedEventHandler ObjectSelected;
    }
}
