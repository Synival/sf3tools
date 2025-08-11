using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using CommonLib.Arrays;
using SF3.ByteData;
using SF3.Models.Files.CHP;
using SF3.Models.Files.CHR;
using SF3.NamedValues;
using SF3.Sprites;
using SF3.Types;
using SF3.Extensions;
using CommonLib.Imaging;
using SF3;

namespace CHRTool {
    public static class ExtractSheets {
        public static int Run(string[] args, string spriteDir, string spritesheetDir, string frameHashLookupsFile) {
            // (any extra options would go here.)

            // Fetch the directory with the game data for ripping spritesheets.
            var gameDataFiles = args.Where(x => !x.StartsWith('-')).ToArray();
            if (args.Length == 0) {
                Console.Error.WriteLine("Missing game data directory");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }
            args = args.Where(x => x.StartsWith('-')).ToArray();

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Console.Error.WriteLine("Unrecognized arguments in 'extract-sheet' command:");
                Console.Error.Write($"    {string.Join(" ", args)}");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go!
            Console.WriteLine($"Extracting spritesheet frames from files/paths(s):");
            foreach (var gameDataFile in gameDataFiles)
                Console.WriteLine($"  {gameDataFile}");

            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine($"Sprite directory:        {spriteDir}");
            Console.WriteLine($"Spritesheet directory:   {spritesheetDir}");
            Console.WriteLine($"Frame hash lookups file: {frameHashLookupsFile}");
            Console.WriteLine("------------------------------------------------------------------------------");

            // Try to create the spritesheet directory if it doesn't exist.
            try {
                if (!Directory.Exists(spritesheetDir))
                    Directory.CreateDirectory(spritesheetDir);
            }
            catch (Exception e) {
                Console.Error.WriteLine($"  Couldn't create spritesheet directory a '{spritesheetDir}':");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            // Fetch the CHR and CHP files to extract frames from.
            var filesList = new List<string>();
            foreach (var file in gameDataFiles) {
                try {
                    if (Directory.Exists(file)) {
                        filesList.AddRange(Directory.GetFiles(file, "*.CHR")
                            .Concat(Directory.GetFiles(file, "*.CHP"))
                            .OrderBy(x => x)
                        );
                    }
                    else
                        filesList.Add(file);
                }
                catch (Exception e) {
                    Console.Error.WriteLine($"  Couldn't get game data file or files at '{file}':");
                    Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                    return 1;
                }
            }
            var files = filesList.ToArray();

            // We don't care about the NameGetterContext or Scenario, since CHRs/CHP are all the same format,
            // and we don't care about any scenario-based resources. Just use Scenario 1.
            var scenario = ScenarioType.Scenario1;
            var nameGetterContext = new NameGetterContext(scenario);
            var framesWritten = new HashSet<string>();

            foreach (var file in files) {
                Console.Write($"Extracting frames from '{Path.GetFileName(file)}'");
                var loadedSpritesheets = new Dictionary<string, Bitmap>();
                try {
                    var bytes = File.ReadAllBytes(file);
                    var byteData = new ByteData(new ByteArray(bytes));

                    // Gather the frame refs and textures from either the CHR or CHP file
                    ExtractInfo[] extractInfos;
                    if (file.ToLower().EndsWith(".chr")) {
                        var chrFile = CHR_File.Create(byteData, nameGetterContext, scenario);
                        extractInfos = GetExtractInfos(chrFile);
                    }
                    else if (file.ToLower().EndsWith(".chp")) {
                        var chpFile = CHP_File.Create(byteData, nameGetterContext, scenario);
                        extractInfos = GetExtractInfos(chpFile);
                    }
                    else {
                        Console.Error.WriteLine($"  Unrecognized extension for '{file}'");
                        continue;
                    }

                    // Perform the extraction!
                    var totalFrames = extractInfos.Length;
                    var framesAdded = ExtractFrames(extractInfos, framesWritten, loadedSpritesheets);
                    var framesSkipped = totalFrames - framesAdded;

                    // Report
                    if (framesSkipped > 0)
                        Console.WriteLine($": {framesAdded} frame(s) extracted, {framesSkipped} frame(s) skipped");
                    else if (framesAdded > 0)
                        Console.WriteLine($": {framesAdded} frame(s) extracted");
                    else
                        Console.WriteLine($": no frames");
                }
                catch (Exception e) {
                    Console.WriteLine();
                    Console.Error.WriteLine($"  Couldn't extract sheets from '{file}':");
                    Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                }
            }

            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine("Done");
            return 0;
        }

        private class ExtractInfo {
            public string Hash;
            public FrameHashLookup FrameRef;
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

        private static int ExtractFrames(ExtractInfo[] extractInfos, HashSet<string> framesWritten, Dictionary<string, Bitmap> loadedSpritesheets) {
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
                        var bitmap = LoadSpritesheet(loadedSpritesheets, spriteDef, frameRef.FrameWidth, frameRef.FrameHeight);
                        if (bitmap != null) {
                            if (bitmap.SetDataAt(frame.SpritesheetX, frame.SpritesheetY, texture.ImageData16Bit)) {
                                updatedSpritesheets.Add(SpriteResources.SpritesheetImageFile(spriteDef.Name, frameRef.FrameWidth, frameRef.FrameHeight));
                                framesAdded++;
                            }
                        }
                    }
                    catch (Exception e) {
                        Console.Error.WriteLine($"  Couldn't write frame '{frameRefTex.FrameRef}':");
                        Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                    }
                }
                framesWritten.Add(hash);
            }

            // Save updated bitmaps.
            foreach (var filename in updatedSpritesheets) {
                try {
                    var bitmap = loadedSpritesheets[filename];
                    bitmap.Save(filename, ImageFormat.Png);
                }
                catch (Exception e) {
                    Console.Error.WriteLine($"  Couldn't save bitmap '{filename}':");
                    Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                }
            }

            return framesAdded;
        }

        private static Bitmap LoadSpritesheet(Dictionary<string, Bitmap> loadedSpritesheets, SpriteDef spriteDef, int frameWidth, int frameHeight) {
            var spritesheetImageFile = SpriteResources.SpritesheetImageFile(spriteDef.Name, frameWidth, frameHeight);
            var spritesheet = spriteDef.Spritesheets[Spritesheet.DimensionsToKey(frameWidth, frameHeight)];

            // If the spritesheet has already been loaded, return it.
            if (loadedSpritesheets.TryGetValue(spritesheetImageFile, out var bitmapOut))
                return bitmapOut;

            // If the spritesheet exists, fetch it.
            if (File.Exists(spritesheetImageFile)) {
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
