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
using CommonLib.NamedValues;
using CommonLib.Logging;
using CommonLib.Types;

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
                Logger.WriteLine("No .CHR or .CHP file(s) or path(s) provided", LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Logger.WriteLine("Unrecognized arguments in 'extract-sheet' command: " + string.Join(" ", args), LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go!
            try {
                if (verbose) {
                    Logger.WriteLine("Extracting frames to spritesheets...");
                    Logger.WriteLine("------------------------------------------------------------------------------");
                }

                // Try to create the spritesheet directory if it doesn't exist.
                if (!Directory.Exists(spritesheetDir)) {
                    if (verbose)
                        Logger.WriteLine($"Creating path '{spritesheetDir}'");
                    Directory.CreateDirectory(spritesheetDir);
                }

                // We don't care about the NameGetterContext or Scenario, since CHRs/CHP are all the same format,
                // and we don't care about any scenario-based resources. Just use Scenario 1.
                var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);

                // Extract!
                var loadedSpritesheets  = new Dictionary<string, Bitmap>();
                var framesWritten       = new HashSet<string>();
                var spritesheetsUpdated = new HashSet<string>();

                var framesAdded = 0;
                var framesUnchanged = 0;
                foreach (var file in files) {
                    try {
                        (var fa, var fs) = ExtractFrames(file, nameGetterContext, loadedSpritesheets, framesWritten, spritesheetsUpdated, verbose);
                        framesAdded += fa;
                        framesUnchanged += fs;
                    }
                    catch (Exception e) {
                        Logger.WriteLine("");
                        Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                    }
                }

                // Save updated bitmaps.
                if (verbose)
                    Logger.WriteLine("------------------------------------------------------------------------------");
                Logger.WriteLine($"Saving {spritesheetsUpdated.Count} updated spritesheet(s)...");
                if (verbose && spritesheetsUpdated.Count > 0)
                    Logger.WriteLine("------------------------------------------------------------------------------");

                foreach (var filename in spritesheetsUpdated) {
                    try {
                        if (verbose)
                            Logger.WriteLine($"Saving '{filename}'...");
                        var bitmap = loadedSpritesheets[filename];
                        bitmap.Save(filename, ImageFormat.Png);
                    }
                    catch (Exception e) {
                        Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                    }
                }

                if (verbose)
                    Logger.WriteLine("------------------------------------------------------------------------------");

                Logger.WriteLine($"{framesAdded} total new frame(s) extracted, {framesUnchanged} total frame(s) found but unchanged");
            }
            catch (Exception e) {
                if (verbose)
                    Logger.WriteLine("------------------------------------------------------------------------------");
                Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                return 1;
            }

            if (verbose) {
                Logger.WriteLine("------------------------------------------------------------------------------");
                Logger.WriteLine("Done");
            }
            return 0;
        }

        private static (int framesAdded, int framesUnchanged) ExtractFrames(string file, INameGetterContext nameGetterContext, Dictionary<string, Bitmap> loadedSpritesheets, HashSet<string> framesWritten, HashSet<string> spritesheetsUpdated, bool verbose) {
            if (!file.ToLower().EndsWith(".chr") && !file.ToLower().EndsWith(".chp"))
                throw new Exception($"File '{file}' is not a .CHR or .CHP file");

            Logger.Write($"Extracting frames from '{file}': " + (verbose ? "\n" : ""));
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
            (var framesAdded, var framesUnchanged) = ExtractFrames(extractInfos, loadedSpritesheets, framesWritten, spritesheetsUpdated, verbose);

            // Report
            var totalNewFrames = framesAdded + framesUnchanged;
            if (totalNewFrames > 0 && framesUnchanged > 0)
                Logger.WriteLine($"{totalNewFrames} new frame(s): {framesAdded} extracted, {framesUnchanged} unchanged");
            else if (totalNewFrames > 0)
                Logger.WriteLine($"{totalNewFrames} new frame(s): {framesAdded} extracted");
            else
                Logger.WriteLine($"no new frames");

            return (framesAdded, framesUnchanged);
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

        private static (int framesAdded, int framesUnchanged) ExtractFrames(ExtractInfo[] extractInfos, Dictionary<string, Bitmap> loadedSpritesheets, HashSet<string> framesWritten, HashSet<string> spritesheetsUpdated, bool verbose) {
            // Get a list of all frames referenced in this CHR file.
            var frameRefs = extractInfos
                .OrderBy(x => x.FrameRef.SpriteName)
                .ThenBy(x => x.FrameRef.FrameWidth)
                .ThenBy(x => x.FrameRef.FrameHeight)
                .ThenBy(x => x.FrameRef.FrameGroupName)
                .ThenBy(x => x.FrameRef.FrameDirection)
                .GroupBy(x => x.Hash)
                .ToDictionary(x => x.Key, x => x.ToArray());

            // Add frames to spritesheet bitmaps.
            int framesAdded = 0;
            int framesUnchanged = 0;

            foreach (var frameRefTexKv in frameRefs) {
                var hash = frameRefTexKv.Key;
                var extractInfo = frameRefTexKv.Value;

                foreach (var frameRefTex in extractInfo) {
                    var frameRef = frameRefTex.FrameRef;
                    var frameRefKey = frameRef.ToString();
                    try {
                        if (framesWritten.Contains(frameRefKey))
                            continue;

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
                            var newData = texture.ImageData16Bit;
                            var existingData = bitmap.GetDataAt(frame.SpritesheetX, frame.SpritesheetY, frameRef.FrameWidth, frameRef.FrameHeight);
                            if (Enumerable.SequenceEqual(newData.To1DArray(), existingData.To1DArray()))
                                framesUnchanged++;
                            else if (bitmap.SetDataAt(frame.SpritesheetX, frame.SpritesheetY, newData)) {
                                framesAdded++;
                                spritesheetsUpdated.Add(SpriteResources.SpritesheetImageFile(spriteDef.Name, frameRef.FrameWidth, frameRef.FrameHeight));
                            }
                        }
                    }
                    catch (Exception e) {
                        Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                    }
                    framesWritten.Add(frameRefKey);
                }
            }

            return (framesAdded, framesUnchanged);
        }

        private static Bitmap LoadSpritesheet(Dictionary<string, Bitmap> loadedSpritesheets, SpriteDef spriteDef, int frameWidth, int frameHeight, bool verbose) {
            var spritesheetImageFile = SpriteResources.SpritesheetImageFile(spriteDef.Name, frameWidth, frameHeight);
            var spritesheet = spriteDef.Spritesheets[Spritesheet.DimensionsToKey(frameWidth, frameHeight)];

            // If the spritesheet has already been loaded, return it.
            if (loadedSpritesheets.TryGetValue(spritesheetImageFile, out var bitmapOut))
                return bitmapOut;

            // If the spritesheet exists, fetch it.
            try {
                if (File.Exists(spritesheetImageFile)) {
                    if (verbose)
                        Logger.WriteLine($"Loading spritesheet '{spritesheetImageFile}...");

                    // We have to load the bitmap in this odd way to prevent exceptions caused by saving to the same file you loaded from...
                    // Pretty cool stuff.
                    var bitmap = new Bitmap(new MemoryStream(File.ReadAllBytes(spritesheetImageFile)));
                    loadedSpritesheets.Add(spritesheetImageFile, bitmap);
                    return bitmap;
                }

                // If the spritesheet doesn't exist, create it, with placeholder red squares for spritesheets.
                // (These will stay red if actual frames aren't found)
                if (verbose)
                    Logger.WriteLine($"Creating new spritesheet '{spriteDef.Name} ({Spritesheet.DimensionsToKey(frameWidth, frameHeight)})...");

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
                if (verbose)
                    Logger.WriteLine($"Saving to '{spritesheetImageFile}'...");
                newImage.Save(spritesheetImageFile, ImageFormat.Png);

                // Cache the loaded image and return it for later editing.
                loadedSpritesheets.Add(spritesheetImageFile, newImage);
                return newImage;
            }
            catch (Exception e) {
                Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                loadedSpritesheets.Add(spritesheetImageFile, null);
                return null;
            }
        }
    }
}
