using System;
using System.IO;
using CommonLib.Extensions;
using CommonLib.Logging;
using CommonLib.Types;
using SF3.Sprites;

namespace CHRTool {
    public static class UpdateHashLookups {
        public static int Run(string[] args, string spriteDir, string frameHashLookupsFile, bool verbose) {
            // (any extra options would go here.)

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Logger.WriteLine("Unrecognized arguments in 'update-hashes' command: " + string.Join(" ", args), LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            int totalFramesAdded = 0;
            try {
                // Fetch all .SF3Sprite files
                if (verbose) {
                    Logger.WriteLine("Updating lookup hashes...");
                    Logger.WriteLine("------------------------------------------------------------------------------");
                    Logger.WriteLine("Getting list of SF3Sprites...");
                }
                var files = Directory.GetFiles(spriteDir, "*.SF3Sprite");

                // Get the curent list of frame hash lookups.
                if (!File.Exists(frameHashLookupsFile)) {
                    if (verbose)
                        Logger.WriteLine($"Couldn't find '{frameHashLookupsFile}' -- creating file from scratch.");
                }
                else {
                    if (verbose)
                        Logger.WriteLine($"Loading '{frameHashLookupsFile}'...");
                    SpriteResources.LoadFrameRefs();
                }

                // Open SF3Sprite files and their accompanying spritesheets.
                if (verbose) {
                    Logger.WriteLine("------------------------------------------------------------------------------");
                    Logger.WriteLine("Checking sprites for new frame hashes:");
                }
                foreach (var file in files) {
                    Logger.Write($"Adding frame hashes from '{file}': ");
                    try {
                        var jsonText = File.ReadAllText(file);
                        var spriteDef = SpriteDef.FromJSON(jsonText);

                        (var framesAdded, var framesSkipped) = SpriteResources.AddFrameRefs(spriteDef);
                        totalFramesAdded += framesAdded;

                        if (framesSkipped > 0)
                            Logger.WriteLine($"{framesAdded} new frame(s) added, {framesSkipped} frame(s) skipped");
                        else if (framesAdded > 0)
                            Logger.WriteLine($"{framesAdded} new frame(s) added");
                        else
                            Logger.WriteLine("no new frames");
                    }
                    catch (Exception e) {
                        Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                    }
                }

                if (verbose)
                    Logger.WriteLine("------------------------------------------------------------------------------");

                if (totalFramesAdded > 0) {
                    Logger.WriteLine($"Writing '{frameHashLookupsFile}' ({totalFramesAdded} new frames)...");
                    SpriteResources.WriteFrameRefsJSON();
                }
                else
                    Logger.WriteLine($"No new frames; not updating frame hash lookup file");
            }
            catch (Exception e) {
                if (verbose)
                    Logger.WriteLine("------------------------------------------------------------------------------");
                Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                return 1;
            }

            if (verbose) {
                Logger.WriteLine("------------------------------------------------------------------------------");
                Logger.WriteLine($"Done");
            }
            return 0;
        }
    }
}
