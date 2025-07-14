using System.Drawing;
using System.Drawing.Imaging;
using CommonLib.Arrays;
using CommonLib.Imaging;
using CommonLib.Utils;
using Newtonsoft.Json;
using SF3.ByteData;
using SF3.Sprites;

namespace CHR_Builder {
    public class Program {
        public const string c_spritePath = "../../../../CHR_Extractor/Private";
        public const string c_outputPath = "../../../Private";

        public static int Main(string[] args) {
            var spriteDirs = Directory.GetDirectories(c_spritePath);
            var spriteDefFiles = spriteDirs
                .SelectMany(x => Directory.GetFiles(x, "*.json"))
                .ToArray();

            var spriteDefsWithPaths = spriteDefFiles
                // TODO: check for invalid objects!
                .Select(x => new { Path = x, SpriteDef = JsonConvert.DeserializeObject<SpriteDef>(File.ReadAllText(x))! })
                .ToArray();

            _ = Directory.CreateDirectory(c_outputPath);
            foreach (var spriteDefWithPath in spriteDefsWithPaths) {
                var path = spriteDefWithPath.Path ?? "";
                var spriteDef = spriteDefWithPath.SpriteDef;
                var outputChrFile = Path.Combine(c_outputPath, $"{FilesystemString(spriteDef.Name)}.CHR");

                Console.WriteLine($"Creating '{outputChrFile}'...");

                using (var fileOut = File.Open(outputChrFile, FileMode.Create)) {
#pragma warning disable CA1416 // Validate platform compatibility
                    var spritesheets = spriteDef.Variants
                        // Sprite sheets don't care about the number of directions, only (width x height).
                        .Select(x => Path.GetFileNameWithoutExtension(path) + $" ({x.Width}x{x.Height})")
                        .Distinct()
                        .Where(x => File.Exists(Path.Combine(Path.GetDirectoryName(path) ?? "", x + ".BMP")))
                        .ToDictionary(x => x, x => {
                            var spritesheetFile = Path.Combine(Path.GetDirectoryName(path) ?? "", x + ".BMP");
                            var image = (Bitmap) Image.FromFile(spritesheetFile);
                            return image;
                        });
#pragma warning restore CA1416 // Validate platform compatibility

                    WriteSpriteDefCHR(fileOut, spriteDef, spritesheets);
                }
            }

            return 0;
        }

        private static void WriteSpriteDefCHR(FileStream fileOut, SpriteDef spriteDef, Dictionary<string, Bitmap> spritesheets) {
            var compressedFrameData = GetCompressedFrameData(spriteDef, spritesheets);

            WriteSpriteHeaderTable(fileOut, spriteDef, out var frameTableOffsetAddrs, out var animationTableOffsetAddrs);
            WriteSpriteAnimationTables(fileOut, spriteDef, compressedFrameData, animationTableOffsetAddrs);
            WriteSpriteFrameTables(fileOut, spriteDef, compressedFrameData, frameTableOffsetAddrs);
        }

        private class CompressedFrameDataInfo {
            public CompressedFrameDataInfo(int variantIndex, int animationIndex, int animationFrameIndex, int animationFrameSubIndex, string hash) {
                VariantIndex           = variantIndex;
                AnimationIndex         = animationIndex;
                AnimationFrameIndex    = animationFrameIndex;
                AnimationFrameSubIndex = animationFrameSubIndex;
                Hash                   = hash;
            }

            public int VariantIndex;
            public int AnimationIndex;
            public int AnimationFrameIndex;
            public int AnimationFrameSubIndex;
            public string Hash;
            public int FrameIndex;
            public int VariantFrameIndex;
            public int CompressedDataIndex;
        }

        private class CompressedFrameData {
            public CompressedFrameData(CompressedFrameDataInfo[] info, byte[][] data) {
                Info = info;
                Data = data;
            }

            public CompressedFrameDataInfo[] Info;
            public byte[][] Data;
        }

        private static CompressedFrameData GetCompressedFrameData(SpriteDef spriteDef, Dictionary<string, Bitmap> spritesheets) {
            byte[] GetCompressedFrame(Bitmap spritesheet, int index, int x, int y, int width, int height) {
#pragma warning disable CA1416 // Validate platform compatibility
                var uncompressedData = new ushort[width * height];
                if (spritesheet == null || x < 0 || y < 0 || !(spritesheet.PixelFormat == PixelFormat.Format16bppArgb1555 || spritesheet.PixelFormat == PixelFormat.Format32bppArgb))
                    return Compression.CompressSpriteData(uncompressedData, 0, uncompressedData.Length);

                var bytesPerPixel = (spritesheet.PixelFormat == PixelFormat.Format32bppArgb) ? 4 : 2;

                var ixMax = Math.Min(spritesheet.Width,  x + width);
                var iyMax = Math.Min(spritesheet.Height, y + height);

#pragma warning disable CA1416 // Validate platform compatibility
                var bitmapData = spritesheet.LockBits(new Rectangle(0, 0, spritesheet.Width, spritesheet.Height), ImageLockMode.ReadOnly, spritesheet.PixelFormat);
                unsafe {
                    var bitmapDataPtr = (byte*) bitmapData.Scan0.ToPointer();
                    int writePos = 0;
                    for (var iy = y; iy < iyMax; iy++) {
                        var readPos = (iy * spritesheet.Width + x) * bytesPerPixel;
                        for (var ix = x; ix < ixMax; ix++) {
                            uint bitmapColor = 0;
                            for (int i = 0; i < bytesPerPixel; i++)
                                bitmapColor |= (uint) (bitmapDataPtr[readPos++] << (i * 8));
                            uncompressedData[writePos++] = (bytesPerPixel == 4)
                                ? PixelConversion.ARGB8888toABGR1555(bitmapColor)
                                : PixelConversion.ARGB1555toABGR1555((ushort) bitmapColor);
                        }
                    }
                }
                spritesheet.UnlockBits(bitmapData);

                return Compression.CompressSpriteData(uncompressedData, 0, uncompressedData.Length);
#pragma warning restore CA1416 // Validate platform compatibility
            }

            var framesByHash = spriteDef.Spritesheets
                .SelectMany(x => x.Value.FrameGroups.SelectMany(y => y.Value.Frames.Select(z => new StandaloneFrameDef(z.Value, y.Value.Name, x.Value.FrameWidth, x.Value.FrameHeight))))
                .ToDictionary(x => x.Hash, x => x);

            var uniqueAnimationFramesByVariant = spriteDef
                .Variants
                .Select(x => (Variant: x, UniqueAnimationFrames: x.Animations
                    .SelectMany(y => y.AnimationFrames)
                    .Where(y => y != null && y.FrameHashes != null)
                    .GroupBy(y => y.FrameHashes.Aggregate((a, b) => a + ((b == null) ? "_" : b)))
                    .Select(y => y.First())
                    .ToHashSet()
                ))
                .ToDictionary(x => x.Variant, x => x.UniqueAnimationFrames);

            var frameInfos = spriteDef.Variants
                .SelectMany((x, xi) => (x.Animations ?? [])
                    .SelectMany((y, yi) => (y.AnimationFrames ?? [])
                        .Where(z => uniqueAnimationFramesByVariant[x].Contains(z))
                        .SelectMany((z, zi) => (z.FrameHashes ?? [])
                            .Where(zz => zz != null)
                            .Select((zz, zzi) => new CompressedFrameDataInfo(variantIndex: xi, animationIndex: yi, animationFrameIndex: zi, animationFrameSubIndex: zzi, hash: zz)))
                        )
                    )
                .ToList();

            var frames = frameInfos
                .Where(x => framesByHash.ContainsKey(x.Hash))
                .Select(x => framesByHash[x.Hash])
                .ToList();

            var uniqueFramesByHash = frames
                .GroupBy(x => x.Hash)
                .ToDictionary(x => x.Key, x => x.First());

            var uniqueFrameHashesFromAnimations = uniqueFramesByHash.Keys.ToHashSet();
            var framesNotUsedInAnimations = spriteDef.Spritesheets
                .SelectMany(x => x.Value.FrameGroups.SelectMany(y => y.Value.Frames.Select(z => new StandaloneFrameDef(z.Value, y.Value.Name, x.Value.FrameWidth, x.Value.FrameHeight))))
                .Where(x => !uniqueFrameHashesFromAnimations.Contains(x.Hash)).ToArray();

            foreach (var f in framesNotUsedInAnimations) {
                var variant = spriteDef.Variants
                    .Select((x, i) => (Variant : (SpriteVariantDef?) x, Index : i))
                    .FirstOrDefault(x => x.Variant?.Width == f.Width && x.Variant?.Height == f.Height);
                if (variant.Variant == null)
                    continue;

                frameInfos.Add(new CompressedFrameDataInfo(variant.Index, -1, -1, -1, f.Hash));
                frames.Add(f);
                uniqueFramesByHash[f.Hash] = f;
            }

            frameInfos = frameInfos
                .OrderBy(x => x.VariantIndex)
                .ThenBy(x => x.AnimationIndex)
                .ThenBy(x => x.AnimationFrameIndex)
                .ThenBy(x => x.AnimationFrameSubIndex)
                .ToList();

            var compressedFrameDataByHash = uniqueFramesByHash.Values
                .Select((x, i) => new { Index = i, FrameDef = x })
                .ToDictionary(x => x.FrameDef.Hash, x => {
                    var spritesheetName = $"{FilesystemString(spriteDef.Name)} ({x.FrameDef.Width}x{x.FrameDef.Height})";
#pragma warning disable CA1416 // Validate platform compatibility
                    Bitmap? spritesheet;
                    spritesheet = spritesheets.TryGetValue(spritesheetName, out var spritesheetOut) ? spritesheetOut : spritesheets.Values.FirstOrDefault();
#pragma warning restore CA1416 // Validate platform compatibility
                    return new { x.Index, Data = GetCompressedFrame(spritesheet, x.Index, x.FrameDef.SpriteSheetX, x.FrameDef.SpriteSheetY, x.FrameDef.Width, x.FrameDef.Height) };
                });

            var compressedFrameData = compressedFrameDataByHash.Select(x => x.Value.Data).ToArray();

            var frameIndex = 0;
            var variantFrameIndices = new Dictionary<int, int>();
            foreach (var frameInfo in frameInfos) {
                if (!variantFrameIndices.ContainsKey(frameInfo.VariantIndex))
                    variantFrameIndices[frameInfo.VariantIndex] = 0;

                frameInfo.FrameIndex = frameIndex++;
                frameInfo.VariantFrameIndex = variantFrameIndices[frameInfo.VariantIndex]++;
                frameInfo.CompressedDataIndex = compressedFrameDataByHash.ContainsKey(frameInfo.Hash) ? compressedFrameDataByHash[frameInfo.Hash].Index : -1;
            }

            return new CompressedFrameData(frameInfos.ToArray(), compressedFrameData);
        }

        private static void WriteSpriteHeaderTable(FileStream fileOut, SpriteDef spriteDef, out int[] frameTableOffsetAddrs, out int[] animationTableOffsetAddrs) {
            var outputData = new ByteData(new ByteArray(0x18));
            frameTableOffsetAddrs     = new int[spriteDef.Variants.Length];
            animationTableOffsetAddrs = new int[spriteDef.Variants.Length];

            for (int i = 0; i < spriteDef.Variants.Length; i++) {
                var variant = spriteDef.Variants[i];

                outputData.SetWord(0x00, 0x0000); // Sprite ID (always 0 for these files)
                outputData.SetWord(0x02, variant.Width);
                outputData.SetWord(0x04, variant.Height);
                outputData.SetByte(0x06, (byte) variant.Directions);
                outputData.SetByte(0x07, 0x00);   // Vertical offset (always 0 for these files)
                outputData.SetByte(0x08, 0x00);   // Unknown0x08 (always 0 for these files)
                outputData.SetByte(0x09, 0x00);   // Collision shadow diameter (always 0 for these files)
                outputData.SetByte(0x0A, 0x00);   // Promotion level (always 0 for these files)
                outputData.SetDouble(0x0C, 0x00010000); // Size
                outputData.SetDouble(0x10, 0x00000000); // Frame table offset (TODO: we should know this by now!)
                outputData.SetDouble(0x14, 0x00000000); // Animation table offset (TODO: we should know this by now!)

                frameTableOffsetAddrs[i]     = (i * 0x18) + 0x10;
                animationTableOffsetAddrs[i] = (i * 0x18) + 0x14;

                fileOut.Write(outputData.Data.GetDataCopyOrReference());
            }

            fileOut.WriteByte(0xFF);
            fileOut.WriteByte(0xFF);
            fileOut.Write(new byte[0x16]);
        }

        private static int GetFrameIdForHashes(CompressedFrameDataInfo[] frames, string[] hashes) {
            bool PositionHasHashes(int pos, int index) {
                return (index == hashes.Length) || (pos < frames.Length && frames[pos].Hash == hashes[index] && PositionHasHashes(pos + 1, index + 1));
            }
            for (int i = 0; i < frames.Length; i++)
                if (PositionHasHashes(i, 0))
                    return i;
            return -1;
        }

        private static void WriteSpriteAnimationTables(FileStream fileOut, SpriteDef spriteDef, CompressedFrameData compressedFrameData, int[] animationTableOffsetAddrs) {
            var frameInfosByVariant = compressedFrameData.Info
                .GroupBy(x => x.VariantIndex)
                .Select(x => x.ToArray())
                .ToArray();

            var byteData = new ByteData(new ByteArray(4));
            int variantIndex = 0;
            foreach (var variant in spriteDef.Variants) {
                var variantFrames = (variantIndex < frameInfosByVariant?.Length) ? frameInfosByVariant[variantIndex] : [];
                var aniFrameTableOffsets = new int[variant.Animations.Length];
                int aniIndex = 0;

                var animations = variant.Animations
                    .Where(x => x.AnimationFrames
                    .All(y => y.FrameHashes == null || y.FrameHashes.All(z => z != null)))
                    .ToArray();

                foreach (var animation in animations) {
                    if (animation.AnimationFrames.Length == 0)
                        continue;
                    var lastCmd = animation.AnimationFrames[animation.AnimationFrames.Length - 1].Command;
                    if (lastCmd != 0xF2 && lastCmd != 0xFE && lastCmd != 0xFF)
                        continue;

                    var aniFrameFrameIds = animation.AnimationFrames
                        .Select((x, i) => (
                            FrameID: x.FrameHashes == null ? (int?) null : GetFrameIdForHashes(variantFrames, x.FrameHashes.Where(x => x != null).ToArray()),
                            Index: i)
                        )
                        .ToArray();
                    if (aniFrameFrameIds.Any(x => x.FrameID == -1))
                        continue;

                    aniFrameTableOffsets[aniIndex] = (int) fileOut.Position;
                    var aniFrameIndex = 0;
                    foreach (var aniFrame in animation.AnimationFrames) {
                        if (aniFrame.Command < 0xF1) {
                            byteData.SetWord(0x00, aniFrameFrameIds[aniFrameIndex].FrameID.Value);
                            byteData.SetWord(0x02, aniFrame.ParameterOrDuration);
                        }
                        else {
                            // TODO: actually support commands here
                            // TODO: going to another animation should be a hash lookup if it isn't already
                            byteData.SetWord(0x00, aniFrame.Command);
                            byteData.SetWord(0x02, aniFrame.ParameterOrDuration);
                        }

                        fileOut.Write(byteData.Data.GetDataCopyOrReference());
                        aniFrameIndex++;
                    }
                    aniIndex++;
                }

                var oldPos = (int) fileOut.Position;
                fileOut.Position = animationTableOffsetAddrs[variantIndex];
                byteData.SetDouble(0, oldPos);
                fileOut.Write(byteData.Data.GetDataCopyOrReference());
                fileOut.Position = oldPos;

                var animationCount = Math.Max(0x10, animations.Length);
                for (int i = 0; i < animationCount; i++) {
                    var aniFrameTableOffset = (i < animations.Length) ? aniFrameTableOffsets[i] : 0;
                    byteData.SetDouble(0, aniFrameTableOffset);
                    fileOut.Write(byteData.Data.GetDataCopyOrReference());
                }

                variantIndex++;
            }
        }

        private static void WriteSpriteFrameTables(FileStream fileOut, SpriteDef spriteDef, CompressedFrameData compressedFrameData, int[] frameTableOffsetAddrs) {
            var frameInfosByVariant = compressedFrameData.Info
                .GroupBy(x => x.VariantIndex)
                .Select(x => x.ToArray())
                .ToArray();

            var offsetIndex = 0;
            var offsets = new int[compressedFrameData.Info.Length];

            var byteData = new ByteData(new ByteArray(4));
            var byteDataRef = byteData.Data.GetDataCopyOrReference();
            int variantIndex = 0;
            foreach (var frameInfos in frameInfosByVariant) {
                var oldPos = (int) fileOut.Position;
                byteData.SetDouble(0, oldPos);
                fileOut.Position = frameTableOffsetAddrs[variantIndex];
                fileOut.Write(byteDataRef);
                fileOut.Position = oldPos;

                foreach (var frameInfo in frameInfos) {
                    byteData.SetDouble(0, frameInfo.FrameIndex);
                    offsets[offsetIndex++] = (int) fileOut.Position;
                    fileOut.Write(byteDataRef);
                }

                // 4 bytes for terminator, 4 bytes for padding.
                byteData.SetDouble(0, 0);
                fileOut.Write(byteDataRef);
                fileOut.Write(byteDataRef);

                variantIndex++;
            }

            int imageIndex = 0;
            foreach (var compressedImage in compressedFrameData.Data) {
                var framesWithThisImage = compressedFrameData.Info.Where(x => x.CompressedDataIndex == imageIndex).ToArray();

                if (framesWithThisImage.Length > 0) {
                    var oldPos = (int) fileOut.Position;
                    foreach (var frame in framesWithThisImage) {
                        fileOut.Position = offsets[frame.FrameIndex];
                        byteData.SetDouble(0, oldPos);
                        fileOut.Write(byteDataRef);
                    }
                    fileOut.Position = oldPos;
                }

                fileOut.Write(compressedImage);
                imageIndex++;
            }

            // Write 1) enough zeroes to reach a size divisible by four, and 2) four padding zeros.
            var pos = (int) fileOut.Position;
            var targetPos = ((pos % 4 == 0) ? pos : pos + (4 - (pos % 4))) + 4;
            for (int i = pos; i < targetPos; i++)
                fileOut.WriteByte(0x00);
        }

        private static string FilesystemString(string str) {
            return str
                .Replace(" | ", ", ")
                .Replace("|", ",")
                .Replace("?", "X")
                .Replace("-", "_")
                .Replace(":", "_")
                .Replace("/", "_");
        }
    }
}
