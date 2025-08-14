using System;
using System.Diagnostics;
using System.IO;
using CommonLib.Extensions;
using SF3.Sprites;

namespace CHRTool {
    public static class UpdateHashLookups {
        public static int Run(string[] args, string spriteDir, string frameHashLookupsFile, bool verbose) {
            // (any extra options would go here.)

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Trace.TraceError("Unrecognized arguments in 'update-hashes' command: " + string.Join(" ", args));
                Trace.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            int totalFramesAdded = 0;
            try {
                // Fetch all .SF3Sprite files
                if (verbose) {
                    Trace.WriteLine("Updating lookup hashes...");
                    Trace.WriteLine("------------------------------------------------------------------------------");
                    Trace.WriteLine("Getting list of SF3Sprites...");
                }
                var files = Directory.GetFiles(spriteDir, "*.SF3Sprite");

                // Get the curent list of frame hash lookups.
                if (!File.Exists(frameHashLookupsFile))
                    Trace.WriteLine($"Couldn't find '{frameHashLookupsFile}' -- creating file from scratch.");
                else {
                    Trace.WriteLine($"Loading '{frameHashLookupsFile}'...");
                    SpriteResources.LoadFrameRefs();
                }

                // Open SF3Sprite files and their accompanying spritesheets.
                if (verbose)
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
                        Trace.TraceError(e.GetTypeAndMessage());
                    }
                }

                if (totalFramesAdded > 0) {
                    Trace.WriteLine($"Writing '{frameHashLookupsFile}' ({totalFramesAdded} new frames)...");
                    SpriteResources.WriteFrameRefsJSON();
                }
                else
                    Trace.WriteLine($"No new frames; not updating frame hash lookup file");
            }
            catch (Exception e) {
                if (verbose)
                    Trace.WriteLine("------------------------------------------------------------------------------");
                Trace.TraceError(e.GetTypeAndMessage());
                return 1;
            }

            if (verbose) {
                Trace.WriteLine("------------------------------------------------------------------------------");
                Trace.WriteLine($"Done");
            }
            return 0;
        }
    }
}
