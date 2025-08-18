using System;
using System.IO;
using CommonLib.Extensions;
using CommonLib.Logging;
using CommonLib.Types;
using NDesk.Options;
using SF3.Sprites;

namespace CHRTool {
    public static class UpdateHashLookups {
        public static int Run(string[] args, string spriteDir, string frameHashLookupsFile, bool verbose) {
            var replaceFile = false;

            // Read any command line options.
            var compileOptions = new OptionSet() {
                { "r|replace", v => replaceFile = true },
            };
            try {
                args = compileOptions.Parse(args).ToArray();
            }
            catch (Exception e) {
                Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                return 1;
            }

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Logger.WriteLine("Unrecognized arguments in 'update-hashes' command: " + string.Join(" ", args), LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            int totalFramesAdded = 0;
            if (verbose)
                Logger.WriteLine("Updating lookup hashes...");
            using (Logger.IndentedSection(verbose ? 1 : 0)) {
                try {
                    // Fetch all .SF3Sprite files
                    string[] files;
                    if (verbose)
                        Logger.WriteLine("Getting list of SF3Sprites...");
                    using (Logger.IndentedSection(verbose ? 1 : 0)) {
                        files = Directory.GetFiles(spriteDir, "*.SF3Sprite");

                        // Get the curent list of frame hash lookups.
                        if (!replaceFile) {
                            if (!File.Exists(frameHashLookupsFile)) {
                                if (verbose)
                                    Logger.WriteLine($"Couldn't find '{frameHashLookupsFile}' -- creating file from scratch");
                            }
                            else {
                                if (verbose)
                                    Logger.WriteLine($"Loading '{frameHashLookupsFile}'...");
                                SpriteResources.LoadFrameRefs();
                            }
                        }
                        else if (verbose)
                            Logger.WriteLine($"Replacing '{frameHashLookupsFile}'");
                    }

                    // Open SF3Sprite files and their accompanying spritesheets.
                    if (verbose)
                        Logger.WriteLine("Checking sprites for new frame hashes:");
                    using (Logger.IndentedSection(verbose ? 1 : 0)) {
                        foreach (var file in files) {
                            Logger.FinishLine();
                            Logger.Write($"Adding frame hashes from '{file}': ");
                            using (Logger.IndentedSection()) {
                                try {
                                    var jsonText = File.ReadAllText(file);
                                    var spriteDef = SpriteDef.FromJSON(jsonText);

                                    (var framesAdded, var framesSkipped) = SpriteResources.AddFrameRefs(spriteDef);
                                    totalFramesAdded += framesAdded;

                                    if (framesSkipped > 0)
                                        Logger.Write($"{framesAdded} new frame(s) added, {framesSkipped} frame(s) skipped\n");
                                    else if (framesAdded > 0)
                                        Logger.Write($"{framesAdded} new frame(s) added\n");
                                    else
                                        Logger.Write("no new frames\n");
                                }
                                catch (Exception e) {
                                    Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                                }
                            }
                        }

                        if (totalFramesAdded > 0) {
                            Logger.WriteLine($"Writing '{frameHashLookupsFile}' ({totalFramesAdded} new frames)...");
                            using (Logger.IndentedSection())
                                SpriteResources.WriteFrameRefsJSON();
                        }
                        else
                            Logger.WriteLine($"No new frames; not updating frame hash lookup file");
                    }
                }
                catch (Exception e) {
                    Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                    return 1;
                }
            }

            if (verbose)
                Logger.WriteLine($"Done");

            return 0;
        }
    }
}
