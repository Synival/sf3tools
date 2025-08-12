using System;
using System.IO;
using SF3.Sprites;

namespace CHRTool {
    public static class UpdateHashLookups {
        public static int Run(string[] args, string spriteDir, string frameHashLookupsFile) {
            // (any extra options would go here.)

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Console.Error.WriteLine("Unrecognized arguments in 'update-hashes' command:");
                Console.Error.WriteLine($"    {string.Join(" ", args)}");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            int totalFramesAdded = 0;
            try {
                Console.WriteLine("Updating lookup hashes...");
                Console.WriteLine("------------------------------------------------------------------------------");

                // Fetch all .SF3Sprite files
                string[] files;
                Console.WriteLine("Getting list of SF3Sprites...");
                files = Directory.GetFiles(spriteDir, "*.SF3Sprite");

                // Get the curent list of frame hash lookups.
                if (!File.Exists(frameHashLookupsFile))
                    Console.WriteLine($"Couldn't find '{frameHashLookupsFile}' -- creating file from scratch.");
                else {
                    Console.WriteLine($"Loading '{frameHashLookupsFile}'...");
                    SpriteResources.LoadFrameRefs();
                }

                // Open SF3Sprite files and their accompanying spritesheets.
                Console.WriteLine("Checking sprites for new frame hashes:");
                foreach (var file in files) {
                    Console.WriteLine($"Updating '{Path.GetFileName(file)}'...");
                    try {
                        var jsonText = File.ReadAllText(file);
                        var spriteDef = SpriteDef.FromJSON(jsonText);
                        var framesAdded = SpriteResources.AddFrameRefs(spriteDef);
                        totalFramesAdded += framesAdded;

                        if (framesAdded > 0)
                            Console.WriteLine($"    {framesAdded} new frame(s) added");
                    }
                    catch (Exception e) {
                        Console.Error.WriteLine($"    Error updating frame hashes:");
                        Console.Error.WriteLine($"        {e.GetType().Name}: {e.Message}");
                    }
                }

                Console.WriteLine($"Writing '{frameHashLookupsFile}'...");
                SpriteResources.WriteFrameRefsJSON();
            }
            catch (Exception e) {
                Console.WriteLine("------------------------------------------------------------------------------");
                Console.Error.WriteLine($"Error:");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine($"Done. {totalFramesAdded} frame(s) added.");
            return 0;
        }
    }
}
