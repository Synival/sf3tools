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

namespace CHRTool {
    public static class ExtractSheets {
        public static int Run(string[] args, string spriteDir, string spritesheetDir, string frameHashLookupsFile) {
            // (any extra options would go here.)

            // Fetch the directory with the game data for ripping spritesheets.
            if (args.Length == 0) {
                Console.Error.WriteLine("Missing game data directory");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }
            var gameDataDir = args[0];
            args = args[1..args.Length];

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Console.Error.WriteLine("Unrecognized arguments in 'extract-sheet' command:");
                Console.Error.Write($"    {string.Join(" ", args)}");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            Console.WriteLine($"Extracting spritesheet frames from path '{gameDataDir}'...");
            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine($"Sprite directory:        {spriteDir}");
            Console.WriteLine($"Spritesheet directory:   {spritesheetDir}");
            Console.WriteLine($"Frame hash lookups file: {frameHashLookupsFile}");
            Console.WriteLine("------------------------------------------------------------------------------");

            string[] files;
            try {
                files = Directory.GetFiles(gameDataDir, "*.CHR")
                    .Concat(Directory.GetFiles(gameDataDir, "*.CHP"))
                    .OrderBy(x => x)
                    .ToArray();
            }
            catch (Exception e) {
                Console.WriteLine("------------------------------------------------------------------------------");
                Console.Error.WriteLine($"  Couldn't get game data files from path '{gameDataDir}':");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            // We don't care about the NameGetterContext or Scenario, since CHRs/CHP are all the same format,
            // and we don't care about any scenario-based resources. Just use Scenario 1.
            var scenario = ScenarioType.Scenario1;
            var nameGetterContext = new NameGetterContext(scenario);

            foreach (var file in files) {
                Console.WriteLine($"Extracting sprites from '{Path.GetFileName(file)}'...");
                try {
                    var bytes = File.ReadAllBytes(file);
                    var byteData = new ByteData(new ByteArray(bytes));

                    if (file.ToLower().EndsWith(".chr")) {
                        var chrFile = CHR_File.Create(byteData, nameGetterContext, scenario);
                        ExtractFromCHR(chrFile);
                    }
                    else if (file.ToLower().EndsWith(".chp")) {
                        var chpFile = CHP_File.Create(byteData, nameGetterContext, scenario);
                        foreach (var chrFile in chpFile.CHR_EntriesByOffset.Values)
                            ExtractFromCHR(chrFile);
                    }
                }
                catch (Exception e) {
                    Console.Error.WriteLine($"  Couldn't extract sheets from '{file}':");
                    Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                }
            }

            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine("Done");
            return 0;
        }

        private static void ExtractFromCHR(ICHR_File chrFile) {
            // TODO: we desperately need some try/catch blocks in here!

            // Get a list of all frames referenced in this CHR file.
            var frameRefs = chrFile.SpriteTable
                .SelectMany(x => x.FrameTable.SelectMany(y => y.FrameRefs.Select(z => (FrameRef: z, Texture: y.Texture))))
                .GroupBy(x => x.FrameRef)
                .Select(x => x.First())
                .Distinct()
                .OrderBy(x => x.FrameRef.SpriteName)
                .ThenBy(x => x.FrameRef.FrameWidth)
                .ThenBy(x => x.FrameRef.FrameHeight)
                .ThenBy(x => x.FrameRef.FrameGroupName)
                .ThenBy(x => x.FrameRef.FrameDirection)
                .ToArray();

            var loadedSpritesheets = new Dictionary<string, Bitmap>();
            foreach (var frameRefTex in frameRefs) {
                var frameRef = frameRefTex.FrameRef;
                var spriteDef = SpriteResources.GetSpriteDef(frameRef.SpriteName);
                var spritesheet = spriteDef.Spritesheets[Spritesheet.DimensionsToKey(frameRef.FrameWidth, frameRef.FrameHeight)];
                var frameGroup = spritesheet.FrameGroupsByName[frameRef.FrameGroupName];
                var frame = frameGroup.Frames[frameRef.FrameDirection];

                // TODO: Add this to the spritesheet!
                var texture = frameRefTex.Texture;
                var bitmap = LoadSpritesheet(loadedSpritesheets, spriteDef, frameRef.FrameWidth, frameRef.FrameHeight);
            }
        }

        private static Bitmap LoadSpritesheet(Dictionary<string, Bitmap> loadedSpritesheets, SpriteDef spriteDef, int frameWidth, int frameHeight) {
            var spritesheetImageFile = SpriteResources.SpritesheetImageFile(spriteDef.Name, frameWidth, frameHeight);
            var spritesheet = spriteDef.Spritesheets[Spritesheet.DimensionsToKey(frameWidth, frameHeight)];

            if (loadedSpritesheets.TryGetValue(spritesheetImageFile, out var bitmapOut))
                return bitmapOut;

            // If the spritesheet doesn't exist, create it, with placeholder red squares for spritesheets.
            // (These will stay red if actual frames aren't found)
            if (File.Exists(spritesheetImageFile)) {
                Bitmap bitmap = null;
                try {
                    bitmap = new Bitmap(spritesheetImageFile);
                }
                catch {
                    loadedSpritesheets.Add(spritesheetImageFile, null);
                    throw;
                }
                loadedSpritesheets.Add(spritesheetImageFile, bitmap);
                return bitmap;
            }

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
            var newImage = new Bitmap(sheetWidth, sheetHeight, PixelFormat.Format16bppArgb1555);

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
