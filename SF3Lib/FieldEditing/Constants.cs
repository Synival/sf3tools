using System;
using System.Collections.Generic;
using System.Linq;
using SF3.Types;

namespace SF3.FieldEditing {
    public static class Constants {
        private static TileLayer TL(TileType tt, string id = "*")
            => new TileLayer(tt, id);

        private static Dictionary<TileType, byte> _defaultTexIdByTileType = new Dictionary<TileType, byte>() {
            { TileType.Grass,     0x01 },
            { TileType.Dirt,      0x29 },
            { TileType.DarkGrass, 0x1C },
            { TileType.Hill,      0x42 },
            { TileType.Mountain,  0x4C },
            { TileType.Peak,      0x4E },
            { TileType.Desert,    0x7C },
            { TileType.River,     0x2A },
            { TileType.Bridge,    0x74 },
            { TileType.Water,     0xFF },
        };

        public static byte GetDefaultTexIdByTileType(TileType tileType)
            => _defaultTexIdByTileType[tileType];

        // TODO: This table should be by texture hash!!
        // TODO: This table should be in some sort of XML file!!
        // TODO: don't put this here!!
        private static readonly Dictionary<byte, TileLayer[]> _baseTileLayersByTexId = new Dictionary<byte, TileLayer[]>() {
            { 0x01, new TileLayer[] {                     TL(TileType.Grass)     } },

            { 0x02, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "R3DC")   } },
            { 0x03, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "R3D4LC") } },
            { 0x04, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "D4LC")   } },
            { 0x05, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "U2R3DC") } },
            { 0x06, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "D4L1UC") } },
            { 0x07, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "U2RC")   } },
            { 0x08, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "L1U2RC") } },
            { 0x09, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "L1UC")   } },
            { 0x0A, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "R23")    } },
            { 0x0B, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "L41")    } },
            { 0x0C, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "U12")    } },
            { 0x0D, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "D34")    } },
            { 0x0E, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "C")      } },
            { 0x0F, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "DRC")    } },
            { 0x10, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "LRC")    } },
            { 0x11, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "DLC")    } },
            { 0x12, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "UDC")    } },
            { 0x13, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "UDC")    } },
            { 0x14, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "URC")    } },
            { 0x15, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "LRC")    } },
            { 0x16, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "ULC")    } },
            { 0x17, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "L1UC")   } },
            { 0x18, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "U2RC")   } },
            { 0x19, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "D4LC")   } },
            { 0x1A, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "R3DC")   } },
            { 0x1B, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "UDC")    } },

            { 0x1C, new TileLayer[] {                     TL(TileType.DarkGrass) } },
            { 0x1D, new TileLayer[] { TL(TileType.Grass), TL(TileType.DarkGrass, "R3DC")   } },
            { 0x1E, new TileLayer[] { TL(TileType.Grass), TL(TileType.DarkGrass, "R3D4LC") } },
            { 0x1F, new TileLayer[] { TL(TileType.Grass), TL(TileType.DarkGrass, "D4LC")   } },
            { 0x20, new TileLayer[] { TL(TileType.Grass), TL(TileType.DarkGrass, "U2R3DC") } },
            { 0x21, new TileLayer[] { TL(TileType.Grass), TL(TileType.DarkGrass, "D4L1UC") } },
            { 0x22, new TileLayer[] { TL(TileType.Grass), TL(TileType.DarkGrass, "U2RC")   } },
            { 0x23, new TileLayer[] { TL(TileType.Grass), TL(TileType.DarkGrass, "L1U2RC") } },
            { 0x24, new TileLayer[] { TL(TileType.Grass), TL(TileType.DarkGrass, "L1UC")   } },
            { 0x25, new TileLayer[] { TL(TileType.Grass), TL(TileType.DarkGrass, "2R3")    } },
            { 0x26, new TileLayer[] { TL(TileType.Grass), TL(TileType.DarkGrass, "4L1")    } },
            { 0x27, new TileLayer[] { TL(TileType.Grass), TL(TileType.DarkGrass, "1U2")    } },
            { 0x28, new TileLayer[] { TL(TileType.Grass), TL(TileType.DarkGrass, "3D4")    } },

            { 0x29, new TileLayer[] {                     TL(TileType.Dirt) } },

            { 0x2A, new TileLayer[] { TL(TileType.River), TL(TileType.Grass, "1U23D4") } },
            { 0x2B, new TileLayer[] { TL(TileType.River), TL(TileType.Grass, "2R34L1") } },
            { 0x2C, new TileLayer[] { TL(TileType.River), TL(TileType.Grass, "1U2R34") } },
            { 0x2D, new TileLayer[] { TL(TileType.River), TL(TileType.Grass, "3D4L12") } },
            { 0x2E, new TileLayer[] { TL(TileType.River), TL(TileType.Grass, "4L1U23") } },
            { 0x2F, new TileLayer[] { TL(TileType.River), TL(TileType.Grass, "2R3D41") } },

            { 0x30, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "4L1U2") } },
            { 0x31, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "1U2")   } },
            { 0x32, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "1U2R3") } },
            { 0x33, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "4L1")   } },
            { 0x34, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "2R3")   } },
            { 0x35, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "3D4L1") } },
            { 0x36, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "3D4")   } },
            { 0x37, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "2R3D4") } },
            { 0x38, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "4L1U2") } },
            { 0x39, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "1U2R3") } },
            { 0x3A, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "3D4L1") } },
            { 0x3B, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "2R3D4") } },
            { 0x3C, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "1")     } },
            { 0x3D, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "2")     } },
            { 0x3E, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "4")     } },
            { 0x3F, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "3")     } },
            { 0x40, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "24")    } },
            { 0x41, new TileLayer[] { TL(TileType.Water), TL(TileType.Grass, "124")   } },

            { 0x42, new TileLayer[] {                         TL(TileType.Hill, "*#")    } },
            { 0x43, new TileLayer[] {                         TL(TileType.Hill, "*$")    } },
            { 0x44, new TileLayer[] {                         TL(TileType.Hill, "*@")    } },
            { 0x45, new TileLayer[] {                         TL(TileType.Hill, "*!")    } },
            { 0x46, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Hill, "CDR3#") } },
            { 0x47, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Hill, "CDL4$") } },
            { 0x48, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Hill, "CUR2@") } },
            { 0x49, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Hill, "CUL1!") } },
            { 0x4A, new TileLayer[] { TL(TileType.DarkGrass), TL(TileType.Hill, "CUR2@") } },
            { 0x4B, new TileLayer[] { TL(TileType.DarkGrass), TL(TileType.Hill, "CUL1!") } },

            { 0x4C, new TileLayer[] {                         TL(TileType.Mountain) } },
            { 0x4D, new TileLayer[] {                         TL(TileType.Mountain) } },

            { 0x4E, new TileLayer[] { TL(TileType.Mountain),  TL(TileType.Peak, "D4L1UC") } },
            { 0x4F, new TileLayer[] { TL(TileType.Mountain),  TL(TileType.Peak, "D4L1UC") } },
            { 0x50, new TileLayer[] { TL(TileType.Mountain),  TL(TileType.Peak, "D4LC")   } },
            { 0x51, new TileLayer[] { TL(TileType.Mountain),  TL(TileType.Peak, "L1UC")   } },

            { 0x52, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Mountain, "D4LC")   } },
            { 0x53, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Mountain, "L1UC")   } },
            { 0x54, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Mountain, "D4L1UC") } },
            { 0x55, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Mountain, "L1U2RC") } },
            { 0x56, new TileLayer[] { TL(TileType.DarkGrass), TL(TileType.Mountain, "L1UC")   } },
            { 0x57, new TileLayer[] { TL(TileType.DarkGrass), TL(TileType.Mountain, "D4L1UC") } },
            { 0x58, new TileLayer[] { TL(TileType.DarkGrass), TL(TileType.Mountain, "L1U2RC") } },
            { 0x59, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Mountain, "D4LC")   } },
            { 0x5A, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Mountain, "L1UC")   } },
            { 0x5B, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Mountain, "D4L1UC") } },
            { 0x5C, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Mountain, "L1U2RC") } },

            { 0x5D, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Hill, "U2R3DC#") } },
            { 0x5E, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Hill, "D4L1UC$") } },
            { 0x5F, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Hill, "U2R3DC@") } },
            { 0x60, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Hill, "D4L1UC!") } },
            { 0x61, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Hill, "R3D4LC#") } },
            { 0x62, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Hill, "R3D4LC$") } },
            { 0x63, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Hill, "L1U2RC@") } },
            { 0x64, new TileLayer[] { TL(TileType.Grass),     TL(TileType.Hill, "L1U2RC!") } },
            { 0x65, new TileLayer[] { TL(TileType.DarkGrass), TL(TileType.Hill, "U2R3DC#") } },
            { 0x66, new TileLayer[] { TL(TileType.DarkGrass), TL(TileType.Hill, "D4L1UC$") } },
            { 0x67, new TileLayer[] { TL(TileType.DarkGrass), TL(TileType.Hill, "D4L1UC!") } },
            { 0x68, new TileLayer[] { TL(TileType.DarkGrass), TL(TileType.Hill, "L1U2RC@") } },
            { 0x69, new TileLayer[] { TL(TileType.DarkGrass), TL(TileType.Hill, "L1U2RC!") } },

            { 0x6A, new TileLayer[] { TL(TileType.Dirt),  TL(TileType.Mountain, "D4LC")    } },
            { 0x6B, new TileLayer[] { TL(TileType.Dirt),  TL(TileType.Mountain, "L1UC")    } },
            { 0x6C, new TileLayer[] { TL(TileType.Dirt),  TL(TileType.Mountain, "D4L1UC")  } },
            { 0x6D, new TileLayer[] { TL(TileType.Dirt),  TL(TileType.Mountain, "L1U2RC")  } },
            { 0x6E, new TileLayer[] { TL(TileType.Dirt),  TL(TileType.Mountain, "D4LC")    } },
            { 0x6F, new TileLayer[] { TL(TileType.Dirt),  TL(TileType.Mountain, "L1UC")    } },
            { 0x70, new TileLayer[] { TL(TileType.Dirt),  TL(TileType.Mountain, "D4L1UC")  } },
            { 0x71, new TileLayer[] { TL(TileType.Dirt),  TL(TileType.Mountain, "L1U2RC")  } },

            { 0x72, new TileLayer[] { TL(TileType.Grass), TL(TileType.Hill, "D4"), TL(TileType.Mountain, "L1UC") } },

            { 0x73, new TileLayer[] { TL(TileType.River), TL(TileType.Grass, "1234L") } },

            { 0x74, new TileLayer[] { TL(TileType.River), TL(TileType.Grass, "2R34L1"), TL(TileType.Bridge, "LRC") } },
            { 0x75, new TileLayer[] { TL(TileType.River), TL(TileType.Grass, "1U23D4"), TL(TileType.Bridge, "UDC") } },

            { 0x78, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "DC"), TL(TileType.Bridge, "UC") } },
            { 0x79, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "UC"), TL(TileType.Bridge, "DC") } },
            { 0x7A, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "LC"), TL(TileType.Bridge, "RC") } },
            { 0x7B, new TileLayer[] { TL(TileType.Grass), TL(TileType.Dirt, "RC"), TL(TileType.Bridge, "LC") } },

            { 0x7C, new TileLayer[] {                    TL(TileType.Desert) } },
            { 0x87, new TileLayer[] { TL(TileType.Dirt), TL(TileType.Desert, "R3DC")   } },
            { 0x88, new TileLayer[] { TL(TileType.Dirt), TL(TileType.Desert, "R3D4LC") } },
            { 0x89, new TileLayer[] { TL(TileType.Dirt), TL(TileType.Desert, "D4LC")   } },
            { 0x8A, new TileLayer[] { TL(TileType.Dirt), TL(TileType.Desert, "U2R3DC") } },
            { 0x8B, new TileLayer[] { TL(TileType.Dirt), TL(TileType.Desert, "D4L1UC") } },
            { 0x8C, new TileLayer[] { TL(TileType.Dirt), TL(TileType.Desert, "U2RC")   } },
            { 0x8D, new TileLayer[] { TL(TileType.Dirt), TL(TileType.Desert, "L1U2RC") } },
            { 0x8E, new TileLayer[] { TL(TileType.Dirt), TL(TileType.Desert, "L1UC")   } },
            { 0x8F, new TileLayer[] { TL(TileType.Dirt), TL(TileType.Desert, "2R3")    } },
            { 0x90, new TileLayer[] { TL(TileType.Dirt), TL(TileType.Desert, "4L1")    } },
            { 0x91, new TileLayer[] { TL(TileType.Dirt), TL(TileType.Desert, "1U2")    } },
            { 0x92, new TileLayer[] { TL(TileType.Dirt), TL(TileType.Desert, "3D4")    } },

            { 0xFF, new TileLayer[] { TL(TileType.Water) } },
        };


        private static readonly Dictionary<TileType, TileType[]> _underlyingTileTypes = new Dictionary<TileType, TileType[]>() {
            { TileType.Water,     new TileType[] { } },
            { TileType.River,     new TileType[] { TileType.Water, TileType.River } },
            { TileType.Grass,     new TileType[] { TileType.Water, } },
            { TileType.Dirt,      new TileType[] { TileType.Water, TileType.Grass } },
            { TileType.DarkGrass, new TileType[] { TileType.Water, TileType.Grass } },
            { TileType.Hill,      new TileType[] { TileType.Water, TileType.Grass } },
            { TileType.Mountain,  new TileType[] { TileType.Water, TileType.Grass } },
            { TileType.Peak,      new TileType[] { TileType.Water, TileType.Grass, TileType.Mountain } },
            { TileType.Desert,    new TileType[] { TileType.Water, TileType.Grass, TileType.Dirt } },
            { TileType.Bridge,    new TileType[] { TileType.Water, TileType.River } },
        };

        public static TileType[] GetTileTypeUnderlyingTypes(TileType tileType)
            => _underlyingTileTypes[tileType];

        private static Dictionary<byte, TileDef[]> _tileDefsByTexId = null;
        public static Dictionary<byte, TileDef[]> GetTileDefDictionary() {
            if (_tileDefsByTexId == null)
                BuildTileLayersDictionary();
            return _tileDefsByTexId;
        }

        private static void BuildTileLayersDictionary() {
            _tileDefsByTexId = new Dictionary<byte, TileDef[]>();
            var orientations = (TileOrientation[]) Enum.GetValues(typeof(TileOrientation));

            for (int i = 0; i < 256; i++) {
                byte texId = (byte) i;
                var layers = GetBaseTileLayersByTexID(texId);
                if (layers == null)
                    continue;

                if (i == 0xFF)
                    _tileDefsByTexId[texId] = new TileDef[] { new TileDef(texId, TileDef.TransformLayers(layers, TileOrientation.Normal), TileOrientation.Normal) };
                else
                    _tileDefsByTexId[texId] = orientations.Select(o => new TileDef(texId, TileDef.TransformLayers(layers, o), o)).ToArray();
            }
        }

        public static bool TileTypeFilledWithOuterBorder(TileType tileType)
            => tileType == TileType.Grass || tileType == TileType.Water;

        public static TileLayer[] GetBaseTileLayersByTexID(byte texId)
            => _baseTileLayersByTexId.TryGetValue(texId, out var rval) ? rval : null;

        private static Dictionary<byte, TileType> _tileTypesByTexId = null;
        public static TileType? GetTileTypeByTexID(byte texId) {
            if (_tileTypesByTexId == null)
                BuildTileTypesByTexID();
            return _tileTypesByTexId.TryGetValue(texId, out var rval) ? rval : (TileType?) null;
        }

        private static void BuildTileTypesByTexID() {
            _tileTypesByTexId = new Dictionary<byte, TileType>(256);
            for (int i = 0; i < 256; i++) {
                byte texId = (byte) i;
                var layers = GetBaseTileLayersByTexID(texId);
                if (layers == null)
                    continue;

                var lastLayerWithCenter = layers.LastOrDefault(x => x.Fill.HasFlag(TileFill.C));
                _tileTypesByTexId[texId] = lastLayerWithCenter?.Type ?? TileType.Water;
            }
        }
    }
}
