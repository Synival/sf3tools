using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Images;
using SF3.Models.Structs.MPD.TextureAnimation;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public partial class MPD_File {
        public static ScenarioType? DetectScenario(IByteData data) {
            try {
                // Get addresses we need to check.
                var headerAddrPtr = data.GetDouble(0x0000) - c_RamAddress;
                var headerAddr    = data.GetDouble(headerAddrPtr) - c_RamAddress;

                var chunk18Addr   = data.GetDouble(0x2090);
                var chunk19Addr   = data.GetDouble(0x2098);
                var palette3Addr  = data.GetDouble(headerAddr + 0x0044);
                var chunk21Addr   = data.GetDouble(0x20A8);

                // Determine some things about this MPD file.
                var hasChunk18  = (chunk18Addr > 0);
                var hasChunk19  = (chunk19Addr > 0);
                var hasPalette3 = (palette3Addr & 0xFFFF0000) == 0x00290000;
                var hasChunk21  = (chunk21Addr > 0);

                // We should be able to accurately detect the scenario.
                // Scenario 3 and the Premium Disk have the same MPD format.
                if (hasPalette3)
                    return ScenarioType.Scenario3;
                else if (hasChunk21)
                    return ScenarioType.Scenario2;
                else if (hasChunk19)
                    return ScenarioType.Scenario1;
                else if (hasChunk18)
                    return ScenarioType.Other;
                else
                    return ScenarioType.Ship2;
            }
            catch {
                return null;
            }
        }

        public Palette CreatePalette(int index, int adjR, int adjG, int adjB) {
            var palette = CreatePalette(index);
            if (adjR != 0 || adjG != 0 || adjB != 0) {
                adjR = adjR * 255 / 31;
                adjG = adjG * 255 / 31;
                adjB = adjB * 255 / 31;

                for (int i = 0; i < palette.Channels.Length; i++) {
                    ref var ch = ref palette.Channels[i];
                    ch.r = (byte) MathHelpers.Clamp(ch.r + adjR, 0, 255);
                    ch.g = (byte) MathHelpers.Clamp(ch.g + adjG, 0, 255);
                    ch.b = (byte) MathHelpers.Clamp(ch.b + adjB, 0, 255);
                }
            }
            return palette;
        }

        public Palette CreatePalette(int index) {
            if (index < 0 || index > 2)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (PaletteTables == null || index >= PaletteTables.Length)
                return new Palette(256);

            var paletteTable = PaletteTables[index];
            if (paletteTable == null)
                return new Palette(256);

            if (paletteTable.Length != 256)
                throw new InvalidOperationException($"PaletteTable[{index}] should be 256 colors, instead it's {paletteTable.Length}");

            return new Palette(paletteTable.Select(x => x.ColorABGR1555).ToArray());
        }

        private struct TextureModelAndTextureByName {
            public object Model;
            public ITextureData Texture;
        }

        public ReplaceTexturesFromFilesResult ReplaceTexturesFromFiles(string[] files, Func<string, ushort[,]> abgr1555ImageDataLoader) {
            var textures1 = (TextureChunks == null) ? new Dictionary<string, TextureModelAndTextureByName>() : TextureChunks
                .Where(x => x != null && x.TextureTable != null)
                .SelectMany(x => x.TextureTable)
                .ToDictionary(x => x.ImportExportName, x => new TextureModelAndTextureByName { Model = x, Texture = x });

            var textures2 = (TextureAnimations == null) ? new Dictionary<string, TextureModelAndTextureByName>() : TextureAnimations
                .SelectMany(x => x.FrameTable)
                .GroupBy(x => x.CompressedImageDataOffset)
                .Select(x => x.First())
                .ToDictionary(x => x.ImportExportName, x => new TextureModelAndTextureByName { Model = x, Texture = x.Texture });

            var textures = textures1.Concat(textures2).ToDictionary(x => x.Key, x => x.Value);

            int succeeded = 0;
            int failed    = 0;
            int missing   = 0;
            int skipped   = 0;

            foreach (var textureKv in textures) {
                var name = textureKv.Key;
                var model = textureKv.Value.Model;
                var texture = textureKv.Value.Texture;

                if (texture.PixelFormat != TexturePixelFormat.ABGR1555) {
                    skipped++;
                    continue;
                }

                var filename = files.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).ToLower() == name.ToLower());
                if (filename == null) {
                    missing++;
                    continue;
                }

                // Try to actually load the texture!
                try {
                    var imageData = abgr1555ImageDataLoader(filename);
                    if (imageData == null) {
                        failed++;
                        continue;
                    }

                    var imageDataWidth  = imageData.GetLength(0);
                    var imageDataHeight = imageData.GetLength(1);
                    if (imageDataWidth != texture.Width || imageDataHeight != texture.Height) {
                        failed++;
                        continue;
                    }

                    // MPD textures should have end codes.
                    // One common texture used for the locked chest is encoded in an ever-so-slightly different way,
                    // so account for that to prevent "IsModified" from always being set.
                    bool applyEndCodesToBorder = true;
                    var tm = model as TextureModel;
                    if (tm != null && tm.ID == 0x109 && tm.ChunkIndex == 12)
                        applyEndCodesToBorder = false;
                    imageData.FixSaturnTransparency(useEndCodes: true, applyEndCodesToBorder);

                    if (tm != null)
                        tm.ImageData16Bit = imageData;
                    else if (model is FrameModel fm) {
                        var referenceTex = TextureChunks.Where(x => x != null).Select(x => x.TextureTable).SelectMany(x => x).FirstOrDefault(x => x.ID == fm.TextureID);
                        _ = fm.UpdateTextureABGR1555(Chunk3Frames.First(x => x.Offset == fm.CompressedImageDataOffset).Data.DecompressedData, imageData, referenceTex);
                    }
                    else
                        throw new NotSupportedException("Not sure what this is, but it's not supported here");

                    succeeded++;
                }
                catch {
                    failed++;
                }
            }

            return new ReplaceTexturesFromFilesResult {
                Replaced = succeeded,
                Missing  = missing,
                Failed   = failed,
                Skipped  = skipped
            };
        }

        public ExportTexturesToPathResult ExportTexturesToPath(string path, Action<string, ushort[,]> abgr1555ImageDataWriter) {
            var textures1 = (TextureChunks == null) ? new Dictionary<string, ITextureData>() : TextureChunks
                .Where(x => x != null && x.TextureTable != null)
                .SelectMany(x => x.TextureTable)
                .ToDictionary(x => x.ImportExportName, x => (ITextureData) x);

            var textures2 = (TextureAnimations == null) ? new Dictionary<string, ITextureData>() : TextureAnimations
                .SelectMany(x => x.FrameTable)
                .GroupBy(x => x.CompressedImageDataOffset)
                .Select(x => x.First())
                .Where(x => x.TextureIsLoaded)
                .ToDictionary(x => x.ImportExportName, x => (ITextureData) x.Texture);

            var textures = textures1.Concat(textures2).ToDictionary(x => x.Key, x => x.Value);

            int succeeded = 0;
            int failed = 0;
            int skipped = 0;

            foreach (var textureKv in textures) {
                var name = textureKv.Key;
                var texture = textureKv.Value;

                var filename = Path.Combine(path, name + ".png");
                try {
                    if (texture.PixelFormat != TexturePixelFormat.ABGR1555)
                        skipped++;
                    abgr1555ImageDataWriter(filename, texture.ImageData16Bit);
                    succeeded++;
                }
                catch {
                    failed++;
                }
            }

            return new ExportTexturesToPathResult {
                Exported = succeeded,
                Failed   = failed,
                Skipped  = skipped
            };
        }
    }
}
