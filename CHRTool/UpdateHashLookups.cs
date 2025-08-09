using System;
using System.IO;
using SF3.Sprites;

namespace CHRTool {
    public static class UpdateHashLookups {
        public static int Run(string[] args, string spriteDir, string spritesheetDir, string frameHashLookupsFile) {
            // (any extra options would go here.)

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Console.Error.WriteLine("Unrecognized arguments in 'update-hashes' command:");
                Console.Error.Write($"    {string.Join(" ", args)}");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            Console.WriteLine("Updating lookup hashes...");
            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine($"Sprite directory:        {spriteDir}");
            Console.WriteLine($"Spritesheet directory:   {spritesheetDir}");
            Console.WriteLine($"Frame hash lookups file: {frameHashLookupsFile}");
            Console.WriteLine("------------------------------------------------------------------------------");

            // Fetch all .SF3Sprite files
            string[] files;
            try {
                Console.WriteLine("Getting list of SF3Sprites...");
                files = Directory.GetFiles(spriteDir, "*.SF3Sprite");
            }
            catch (Exception e) {
                Console.WriteLine("------------------------------------------------------------------------------");
                Console.Error.WriteLine($"  Couldn't get SF3Sprite files from path '{spriteDir}':");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            // Get the curent list of frame hash lookups.
            try {
                if (!File.Exists(frameHashLookupsFile))
                    Console.WriteLine($"Couldn't find '{frameHashLookupsFile}'. Creating file from scratch.");
                else {
                    Console.WriteLine($"Loading '{frameHashLookupsFile}'...");
                    SpriteResources.LoadFrameHashLookups();
                }
            }
            catch (Exception e) {
                Console.WriteLine("------------------------------------------------------------------------------");
                Console.Error.WriteLine($"  Couldn't fetch current frame hash lookups:");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                Console.Error.WriteLine($"  The file has likely been corrupted. Consider deleting it and trying again");
                return 1;
            }

            // Open SF3Sprite files and their accompanying spritesheets.
            int totalFramesAdded = 0;
            Console.WriteLine("Checking sprites for new frame hashes:");
            foreach (var file in files) {
                Console.WriteLine($"  {Path.GetFileName(file)}");
                try {
                    var jsonText = File.ReadAllText(file);
                    var spriteDef = SpriteDef.FromJSON(jsonText);
                    var framesAdded = SpriteResources.AddFrameHashLookups(spriteDef);
                    totalFramesAdded += framesAdded;

                    if (framesAdded > 0)
                        Console.WriteLine($"    {framesAdded} frame(s) added");
                }
                catch (Exception e) {
                    Console.WriteLine();
                    Console.Error.WriteLine($"  Couldn't update hashes sheets for '{file}':");
                    Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                }
            }

            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine($"Writing '{frameHashLookupsFile}'...");
            try {
                SpriteResources.WriteFrameHashLookupsJSON();
            }
            catch (Exception e) {
                Console.WriteLine();
                Console.Error.WriteLine($"  Couldn't write '{frameHashLookupsFile}':");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
            }

            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine($"Done. {totalFramesAdded} frame(s) added.");
            return 0;
        }
    }
}
