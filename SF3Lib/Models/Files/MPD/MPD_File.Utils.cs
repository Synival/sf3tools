using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Images;
using SF3.Models.Structs.MPD.Main;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Models.Structs.Shared;
using SF3.Models.Tables;
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

        public ReplaceTexturesFromFilesResult ReplaceTexturesFromFiles(string[] files, Func<string, ushort[,]> abgr1555ImageDataLoader) {
            var textures1 = (TextureChunks == null) ? new Dictionary<string, ITexture>() : TextureChunks
                .Where(x => x != null && x.TextureTable != null)
                .SelectMany(x => x.TextureTable)
                .ToDictionary(x => x.ImportExportName, x => (ITexture) x);

            var textures2 = (TextureAnimations == null) ? new Dictionary<string, ITexture>() : TextureAnimations
                .SelectMany(x => x.TextureAnimationFrameTable)
                .GroupBy(x => x.ImageDataOffset)
                .Select(x => x.First())
                .ToDictionary(x => x.ImportExportName, x => (ITexture) x);

            var textures = textures1.Concat(textures2).ToDictionary(x => x.Key, x => x.Value);

            int succeeded = 0;
            int failed    = 0;
            int missing   = 0;
            int skipped   = 0;

            foreach (var textureKv in textures) {
                var name = textureKv.Key;
                var texture = textureKv.Value;

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
                    if (texture != null && texture.ID == 0x109 && texture is TextureStruct tsb && tsb.ChunkIndex == 12)
                        applyEndCodesToBorder = false;
                    imageData.FixSaturnTransparency(useEndCodes: true, applyEndCodesToBorder);

                    texture.ImageData16Bit = imageData;
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
            var textures1 = (TextureChunks == null) ? new Dictionary<string, ITexture>() : TextureChunks
                .Where(x => x != null && x.TextureTable != null)
                .SelectMany(x => x.TextureTable)
                .ToDictionary(x => x.ImportExportName, x => (ITexture) x);

            var textures2 = (TextureAnimations == null) ? new Dictionary<string, ITexture>() : TextureAnimations
                .SelectMany(x => x.TextureAnimationFrameTable)
                .GroupBy(x => x.ImageDataOffset)
                .Select(x => x.First())
                .ToDictionary(x => x.ImportExportName, x => (ITexture) x);

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

        private void MarkAllocatedSpace(bool[] usedSpace, int start, int stop) {
            for (int i = 0; i < stop; ++i)
                usedSpace[i] = true;
        }

        private bool[] MarkAllocatedSpace(bool[] usedSpace, IEnumerable<ITable> existingTables) {
            foreach (var table in existingTables)
                MarkAllocatedSpace(usedSpace, table.Address, table.Address + table.SizeInBytesPlusTerminator);
            return usedSpace;
        }


        private bool[] GetUsedHeaderSpace(MPD_Header header, IEnumerable<ITable> existingTables) {
            var usedSpace = new bool[0x2000];

            // Mark '**header', '*header', and 'header'.
            MarkAllocatedSpace(usedSpace, 0, 4);                                         // double-pointer to header
            var headerPtr = Data.GetDouble(0) - 0x290000;                                // pointer to...
            MarkAllocatedSpace(usedSpace, headerPtr, headerPtr + 4);                     //    ...header
            MarkAllocatedSpace(usedSpace, header.Address, header.Address + header.Size); // header

            // Mark all referenced tables
            MarkAllocatedSpace(usedSpace, existingTables);

            return usedSpace;
        }

        private ushort[] GetContiguousUnusedHeaderSpace(bool[] usedSpace) {
            var contiguousUnusedBytes = new ushort[usedSpace.Length];
            MarkContiguousUnusedHeaderSpace(contiguousUnusedBytes, usedSpace);
            return contiguousUnusedBytes;
        }

        private void MarkContiguousUnusedHeaderSpace(ushort[] contiguousUnusedBytes, bool[] usedSpace) {
            if (contiguousUnusedBytes.Length != usedSpace.Length)
                throw new ArgumentException($"{nameof(contiguousUnusedBytes)} should be " +
                    $"{usedSpace.Length} (0x{usedSpace.Length:X4}) bytes, not " +
                    $"{contiguousUnusedBytes.Length} (0x{contiguousUnusedBytes.Length:X4})"
            );

            // Mark contiguous unused bytes by incrementing a counter in reverse.
            ushort contiguousCount = 0;
            for (int i = usedSpace.Length - 1; i >= 0; --i) {
                if (!usedSpace[i])
                    contiguousUnusedBytes[i] = ++contiguousCount;
                else {
                    contiguousUnusedBytes[i] = 0;
                    contiguousCount = 0;
                }
            }
        }
    }
}
