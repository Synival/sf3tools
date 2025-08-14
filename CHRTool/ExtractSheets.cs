using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Extensions;
using SF3.ByteData;
using SF3.Models.Files.CHP;
using SF3.Models.Files.CHR;
using SF3.NamedValues;
using SF3.Sprites;
using SF3.Types;
using SF3.Extensions;
using CommonLib.Imaging;
using SF3;
using System.Diagnostics;
using CommonLib.NamedValues;

namespace CHRTool {
    public static class ExtractSheets {
        public static int Run(string[] args, string spritesheetDir, bool verbose) {
            // (any extra options would go here.)

            // Fetch the directory with the game data for ripping spritesheets.
            string[] files;
            (files, args) = Utils.GetFilesAndPathsFromAgs(args, path => {
                return Directory.GetFiles(path, "*.CHR")
                    .Concat(Directory.GetFiles(path, "*.CHP"))
                    .OrderBy(x => x)
                    .ToArray();
            });
            if (files.Length == 0) {
                Trace.TraceError("No .CHR or .CHP file(s) or path(s) provided");
                Trace.Write(Constants.ErrorUsageString);
                return 1;
            }

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Trace.TraceError("Unrecognized arguments in 'extract-sheet' command: " + string.Join(" ", args));
                Trace.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go!
            try {
                // Try to create the spritesheet directory if it doesn't exist.
                if (!Directory.Exists(spritesheetDir))
                    Directory.CreateDirectory(spritesheetDir);

                // We don't care about the NameGetterContext or Scenario, since CHRs/CHP are all the same format,
                // and we don't care about any scenario-based resources. Just use Scenario 1.
                var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);

                // Extract!
                var framesWritten = new HashSet<string>();
                foreach (var file in files) {
                    try {
                        ExtractFrames(file, nameGetterContext, framesWritten, verbose);
                    }
                    catch (Exception e) {
                        Trace.WriteLine("");
                        Trace.TraceError(e.GetTypeAndMessage());
                    }
                }
            }
            catch (Exception e) {
                if (verbose)
                    Trace.WriteLine("------------------------------------------------------------------------------");
                Trace.TraceError(e.GetTypeAndMessage());
                return 1;
            }

            if (verbose) {
                Trace.WriteLine("------------------------------------------------------------------------------");
                Trace.WriteLine("Done");
            }
            return 0;
        }

        private static void ExtractFrames(string file, INameGetterContext nameGetterContext, HashSet<string> framesWritten, bool verbose) {
            if (!file.ToLower().EndsWith(".chr") && !file.ToLower().EndsWith(".chp"))
                throw new Exception($"File '{file}' is not a .CHR or .CHP file");

            Trace.Write($"Extracting frames from '{file}': ");
            var loadedSpritesheets = new Dictionary<string, Bitmap>();
            var bytes = File.ReadAllBytes(file);
            var byteData = new ByteData(new ByteArray(bytes));

            // Gather the frame refs and textures from either the CHR or CHP file
            ExtractInfo[] extractInfos;
            if (file.ToLower().EndsWith(".chr")) {
                var chrFile = CHR_File.Create(byteData, nameGetterContext, ScenarioType.Scenario1);
                extractInfos = GetExtractInfos(chrFile);
            }
            else {
                var chpFile = CHP_File.Create(byteData, nameGetterContext, ScenarioType.Scenario1);
                extractInfos = GetExtractInfos(chpFile);
            }

            // Perform the extraction!
            var totalFrames = extractInfos.Length;
            var framesAdded = ExtractFrames(extractInfos, framesWritten, loadedSpritesheets, verbose);
            var framesSkipped = totalFrames - framesAdded;

            // Report
            if (framesSkipped > 0)
                Trace.WriteLine($"{framesAdded} frame(s) extracted, {framesSkipped} frame(s) skipped");
            else if (framesAdded > 0)
                Trace.WriteLine($"{framesAdded} frame(s) extracted");
            else
                Trace.WriteLine($"no frames");
        }

        private class ExtractInfo {
            public string Hash;
            public FrameRef FrameRef;
            public ITexture Texture;
        }

        private static ExtractInfo[] GetExtractInfos(ICHR_File chrFile) {
            return chrFile.SpriteTable
                .Where(x => x.FrameTable != null)
                .SelectMany(x => x.FrameTable
                    .SelectMany(y => y.FrameRefs
                        .Where(z => z.FrameGroupName != "(Unknown)")
                        .Select(z => new ExtractInfo() { Hash = y.TextureHash, FrameRef = z, Texture = y.Texture })
                    )
                )
                .GroupBy(x => x.FrameRef)
                .Select(x => x.First())
                .ToArray();
        }

        private static ExtractInfo[] GetExtractInfos(ICHP_File chrFile) {
            return chrFile.CHR_EntriesByOffset.Values
                .SelectMany(w => w.SpriteTable
                    .Where(x => x.FrameTable != null)
                    .SelectMany(x => x.FrameTable
                        .SelectMany(y => y.FrameRefs
                            .Where(z => z.FrameGroupName != "(Unknown)")
                            .Select(z => new ExtractInfo() { Hash = y.TextureHash, FrameRef = z, Texture = y.Texture })
                        )
                    )
                )
                .GroupBy(x => x.FrameRef)
                .Select(x => x.First())
                .ToArray();
        }

        private static int ExtractFrames(ExtractInfo[] extractInfos, HashSet<string> framesWritten, Dictionary<string, Bitmap> loadedSpritesheets, bool verbose) {
            // Get a list of all frames referenced in this CHR file.
            var frameRefs = extractInfos
                .Where(x => !framesWritten.Contains(x.Hash))
                .OrderBy(x => x.FrameRef.SpriteName)
                .ThenBy(x => x.FrameRef.FrameWidth)
                .ThenBy(x => x.FrameRef.FrameHeight)
                .ThenBy(x => x.FrameRef.FrameGroupName)
                .ThenBy(x => x.FrameRef.FrameDirection)
                .GroupBy(x => x.Hash)
                .ToDictionary(x => x.Key, x => x.ToArray());

            // Add frames to spritesheet bitmaps.
            int framesAdded = 0;
            var updatedSpritesheets = new HashSet<string>();
            foreach (var frameRefTexKv in frameRefs) {
                var hash = frameRefTexKv.Key;
                foreach (var frameRefTex in frameRefTexKv.Value) {
                    try {
                        var frameRef = frameRefTex.FrameRef;
                        var spriteDef = SpriteResources.GetSpriteDef(frameRef.SpriteName);
                        var spritesheet = spriteDef.Spritesheets[Spritesheet.DimensionsToKey(frameRef.FrameWidth, frameRef.FrameHeight)];
                        var frameGroup = spritesheet.FrameGroupsByName[frameRef.FrameGroupName];
                        var frame = frameGroup.Frames[frameRef.FrameDirection];

                        if (frame.SpritesheetX < 0 || frame.SpritesheetY < 0)
                            continue;

                        // Add this to the spritesheet!
                        var texture = frameRefTex.Texture;
                        var bitmap = LoadSpritesheet(loadedSpritesheets, spriteDef, frameRef.FrameWidth, frameRef.FrameHeight, verbose);
                        if (bitmap != null) {
                            if (bitmap.SetDataAt(frame.SpritesheetX, frame.SpritesheetY, texture.ImageData16Bit)) {
                                updatedSpritesheets.Add(SpriteResources.SpritesheetImageFile(spriteDef.Name, frameRef.FrameWidth, frameRef.FrameHeight));
                                framesAdded++;
                            }
                        }
                    }
                    catch (Exception e) {
                        Trace.TraceError(e.GetTypeAndMessage());
                    }
                }
                framesWritten.Add(hash);
            }

            // Save updated bitmaps.
            foreach (var filename in updatedSpritesheets) {
                try {
                    if (verbose)
                        Trace.WriteLine($"Saving updated '{filename}'...");
                    var bitmap = loadedSpritesheets[filename];
                    bitmap.Save(filename, ImageFormat.Png);
                }
                catch (Exception e) {
                    Trace.TraceError(e.GetTypeAndMessage());
                }
            }

            return framesAdded;
        }

        private static Bitmap LoadSpritesheet(Dictionary<string, Bitmap> loadedSpritesheets, SpriteDef spriteDef, int frameWidth, int frameHeight, bool verbose) {
            var spritesheetImageFile = SpriteResources.SpritesheetImageFile(spriteDef.Name, frameWidth, frameHeight);
            var spritesheet = spriteDef.Spritesheets[Spritesheet.DimensionsToKey(frameWidth, frameHeight)];

            // If the spritesheet has already been loaded, return it.
            if (loadedSpritesheets.TryGetValue(spritesheetImageFile, out var bitmapOut))
                return bitmapOut;

            // If the spritesheet exists, fetch it.
            if (File.Exists(spritesheetImageFile)) {
                if (verbose)
                    Trace.WriteLine($"Loading spritesheet '{spritesheetImageFile}...");
                try {
                    // We have to load the bitmap in this odd way to prevent exceptions caused by saving to the same file you loaded from...
                    // Pretty cool stuff.
                    var bitmap = new Bitmap(new MemoryStream(File.ReadAllBytes(spritesheetImageFile)));
                    loadedSpritesheets.Add(spritesheetImageFile, bitmap);
                    return bitmap;
                }
                catch {
                    loadedSpritesheets.Add(spritesheetImageFile, null);
                    throw;
                }
            }

            // If the spritesheet doesn't exist, create it, with placeholder red squares for spritesheets.
            // (These will stay red if actual frames aren't found)
            if (verbose)
                Trace.WriteLine($"Creating new spritesheet '{spriteDef.Name} ({Spritesheet.DimensionsToKey(frameWidth, frameHeight)})...");

            // Get the dimensions of the spritesheet.
            var frameDimensions = spritesheet.FrameGroupsByName.Values
                .SelectMany(y => y.Frames.Values
                    .Where(z => z.SpritesheetX >= 0 && z.SpritesheetY >= 0)
                    .Select(z => (X: z.SpritesheetX, Y: z.SpritesheetY))
                )
                .ToArray();
            var sheetWidth  = frameDimensions.Max(x => x.X) + frameWidth;
            var sheetHeight = frameDimensions.Max(x => x.Y) + frameHeight;

            // Create the empty spritesheet.
            var newImage = new Bitmap(sheetWidth, sheetHeight, PixelFormat.Format32bppArgb);

            // Build a red, non-filled rectangle with lines 3 pixels wide.
            var box = new ushort[frameWidth, frameHeight];
            var redColor    = new PixelChannels() { a = 255, r = 255, g = 0,   b = 0 }.ToABGR1555();
            var orangeColor = new PixelChannels() { a = 255, r = 255, g = 127, b = 0 }.ToABGR1555();

            for (int i = 0; i < 3; i++) {
                if (i < frameHeight) {
                    for (int x = i; x < frameWidth - i; ++x) {
                        box[x, i] = redColor;
                        box[x, frameHeight - i - 1] = orangeColor;
                    }
                }
                if (i < frameWidth) {
                    for (int y = i + 1; y < frameHeight - i - 1; ++y) {
                        box[i, y] = redColor;
                        box[frameWidth - i - 1, y] = orangeColor;
                    }
                }
            }

            // Place that red box at all frame locations.
            foreach (var frameGroup in spritesheet.FrameGroupsByName.Values)
                foreach (var frame in frameGroup.Frames.Values)
                    newImage.SetDataAt(frame.SpritesheetX, frame.SpritesheetY, box);

            // Save the image out.
            try {
                if (verbose)
                    Trace.WriteLine($"Saving to '{spritesheetImageFile}'...");
                newImage.Save(spritesheetImageFile, ImageFormat.Png);
            }
            catch {
                loadedSpritesheets.Add(spritesheetImageFile, null);
                throw;
            }

            // Cache the loaded image and return it for later editing.
            loadedSpritesheets.Add(spritesheetImageFile, newImage);
            return newImage;
        }
    }
}
