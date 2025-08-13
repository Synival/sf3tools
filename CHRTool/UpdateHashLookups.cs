using System;
using System.Diagnostics;
using System.IO;
using SF3.Sprites;

namespace CHRTool {
    public static class UpdateHashLookups {
        public static int Run(string[] args, string spriteDir, string frameHashLookupsFile) {
            // (any extra options would go here.)

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Trace.TraceError("Unrecognized arguments in 'update-hashes' command:");
                Trace.TraceError($"    {string.Join(" ", args)}");
                Trace.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            int totalFramesAdded = 0;
            try {
                Trace.WriteLine("Updating lookup hashes...");
                Trace.WriteLine("------------------------------------------------------------------------------");

                // Fetch all .SF3Sprite files
                string[] files;
                Trace.WriteLine("Getting list of SF3Sprites...");
                files = Directory.GetFiles(spriteDir, "*.SF3Sprite");

                // Get the curent list of frame hash lookups.
                if (!File.Exists(frameHashLookupsFile))
                    Trace.WriteLine($"Couldn't find '{frameHashLookupsFile}' -- creating file from scratch.");
                else {
                    Trace.WriteLine($"Loading '{frameHashLookupsFile}'...");
                    SpriteResources.LoadFrameRefs();
                }

                // Open SF3Sprite files and their accompanying spritesheets.
                Trace.WriteLine("Checking sprites for new frame hashes:");
                foreach (var file in files) {
                    Trace.Write($"Updating '{file}': ");
                    try {
                        var jsonText = File.ReadAllText(file);
                        var spriteDef = SpriteDef.FromJSON(jsonText);

                        (var framesAdded, var framesSkipped) = SpriteResources.AddFrameRefs(spriteDef);
                        totalFramesAdded += framesAdded;

                        if (framesSkipped > 0)
                            Trace.WriteLine($"{framesAdded} new frame(s) added, {framesSkipped} frame(s) skipped");
                        else if (framesAdded > 0)
                            Trace.WriteLine($"{framesAdded} new frame(s) added");
                        else
                            Trace.WriteLine("");
                    }
                    catch (Exception e) {
                        Trace.TraceError($"    Error updating frame hashes:");
                        Trace.TraceError($"        {e.GetType().Name}: {e.Message}");
                    }
                }

                Trace.WriteLine($"Writing '{frameHashLookupsFile}'...");
                SpriteResources.WriteFrameRefsJSON();
            }
            catch (Exception e) {
                Trace.WriteLine("------------------------------------------------------------------------------");
                Trace.TraceError($"Error:");
                Trace.TraceError($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            Trace.WriteLine("------------------------------------------------------------------------------");
            Trace.WriteLine($"Done. {totalFramesAdded} frame(s) added.");
            return 0;
        }
    }
}
