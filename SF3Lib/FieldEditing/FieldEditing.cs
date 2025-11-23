using System;
using System.Collections.Generic;
using System.Linq;
using SF3.Models.Files.MPD;
using SF3.Types;
using static SF3.FieldEditing.Constants;

namespace SF3.FieldEditing {
    /// <summary>
    /// NOTE: Everything here is an experimental proof-of-concept for editing. This will likely not reflect how it really works.
    ///       It only works with a very modified version of FIELD.MPD from the Premium Disk.
    /// </summary>
    public static class FieldEditing {
        public static void UpdateTileTextures(Tile[,] tiles, bool setDefaultIfNothingFound) {
            var maxX = tiles.GetLength(0);
            var maxY = tiles.GetLength(1);

            for (int iy = 0; iy < maxY; iy++)
                for (int ix = 0; ix < maxX; ix++)
                    UpdateTileTexture(tiles[ix, iy], setDefaultIfNothingFound);
        }

        public static void UpdateTileTexture(Tile tile, bool setDefaultIfNothingFound) {
            var tileType = GetTileTypeByTexID(tile.TextureID);
            if (tileType.HasValue)
                SetTileTexture(tile, tileType.Value, setDefaultIfNothingFound);
        }

        public static void SetTileTexture(Tile tile, TileType tileType, bool setDefaultIfNothingFound) {
            var tiles = tile.MPD_File.Tiles;
            var gridTileTypes = new TileType[3, 3];
            var defaultTexId = GetDefaultTexIdByTileType(tileType);

            for (var iy = -1; iy <= 1; iy++) {
                var ty = tile.Y + iy;
                for (var ix = -1; ix <= 1; ix++) {
                    var tx = tile.X + ix;
                    var texId =
                        (ix == 0 && iy == 0) ? defaultTexId :
                        (tx >= 0 && ty >= 0 && tx < 64 && ty < 64) ? tiles[tx, ty].TextureID : (byte) 0xFF;
                    gridTileTypes[ix + 1, iy + 1] = GetTileTypeByTexID(texId) ?? TileType.Water;
                }
            }

            var tileLayers = new Dictionary<TileType, TileLayer>();
            var filledWithOuterBorder = TileTypeFilledWithOuterBorder(tileType);
            var underlyingTypes = GetTileTypeUnderlyingTypes(tileType);
            var topUnderlyingType = underlyingTypes.LastOrDefault();

            foreach (var ut in underlyingTypes)
                tileLayers[tileType] = new TileLayer(tileType, TileFill.Full);
            tileLayers[tileType] = new TileLayer(tileType, filledWithOuterBorder ? TileFill.Full : TileFill.C);

            for (int iy = -1; iy <= 1; iy++) {
                for (int ix = -1; ix <= 1; ix++) {
                    if (ix == 0 && iy == 0)
                        continue;

                    var neighborTileType = gridTileTypes[ix + 1, iy + 1];
                    var neighborTileTypes = new List<TileType>();

                    neighborTileTypes.AddRange(GetTileTypeUnderlyingTypes(neighborTileType));
                    neighborTileTypes.Add(neighborTileType);

                    foreach (var ntt in neighborTileTypes) {
                        if (ntt == tileType || (ntt > topUnderlyingType && TileTypeFilledWithOuterBorder(ntt))) {
                            if (!tileLayers.ContainsKey(ntt))
                                tileLayers[ntt] = new TileLayer(ntt, TileFill.None);
                            tileLayers[ntt].Fill |= TileFillExtensions.GetTileFillBitForPosition(ix, iy);
                        }
                    }
                }
            }

            foreach (var layerKv in tileLayers) {
                var layer = layerKv.Value;

                if (TileTypeFilledWithOuterBorder(layer.Type)) {
                    if (layer.Fill.HasFlag(TileFill.U))
                        layer.Fill |= TileFill.UL | TileFill.UR;
                    if (layer.Fill.HasFlag(TileFill.R))
                        layer.Fill |= TileFill.UR | TileFill.DR;
                    if (layer.Fill.HasFlag(TileFill.D))
                        layer.Fill |= TileFill.DR | TileFill.DL;
                    if (layer.Fill.HasFlag(TileFill.L))
                        layer.Fill |= TileFill.DL | TileFill.UL;
                }
                else {
                    if (layer.Fill.HasFlag(TileFill.UL) && !layer.Fill.HasFlag(TileFill.U | TileFill.L))
                        layer.Fill &= ~TileFill.UL;
                    if (layer.Fill.HasFlag(TileFill.UR) && !layer.Fill.HasFlag(TileFill.U | TileFill.R))
                        layer.Fill &= ~TileFill.UR;
                    if (layer.Fill.HasFlag(TileFill.DR) && !layer.Fill.HasFlag(TileFill.D | TileFill.R))
                        layer.Fill &= ~TileFill.DR;
                    if (layer.Fill.HasFlag(TileFill.DL) && !layer.Fill.HasFlag(TileFill.D | TileFill.L))
                        layer.Fill &= ~TileFill.DL;
                }
            }

            var allTileDefs = GetTileDefDictionary();
            var flattenedLayers = new FlattenedLayers(tileLayers.Values.ToArray(), topUnderlyingType);
            var validTileDefs = allTileDefs.SelectMany(ds => ds.Value.Where(d => d.FlattenedLayers == flattenedLayers)).ToArray();

            if (validTileDefs.Length == 0) {
                if (setDefaultIfNothingFound) {
                    tile.TextureID     = defaultTexId;
                    tile.TextureFlip   = TextureFlipType.NoFlip;
                    tile.TextureRotate = TextureRotateType.NoRotation;
                }
                return;
            }

            var normalTileDefs = validTileDefs.Where(d => d.Orientation == TileOrientation.Normal).ToArray();
            if (normalTileDefs.Length > 0)
                validTileDefs = normalTileDefs;

            //var validTilesStr = string.Join(", ", validTileDefs.Select(d => d.ToString()));
            var random = new Random(tile.RandomSeed);

            var tileDef = validTileDefs[random.Next(validTileDefs.Length)];
            tile.TextureID     = tileDef.TexID;
            tile.TextureFlip   = tileDef.Orientation.GetTextureFlip();
            tile.TextureRotate = tileDef.Orientation.GetTextureRotate();
        }
    }
}
