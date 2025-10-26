using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Logging;
using CommonLib.NamedValues;
using CommonLib.Types;
using SF3.CHR;
using SF3.NamedValues;
using SF3.Sprites;
using SF3.Types;

namespace CHRTool {
    public static class Depends {
        public static int Run(string[] args, bool verbose) {
            // Fetch the directory with the game data.
            string[] files;
            (files, args) = Utils.GetFilesAndPathsFromAgs(args, path => {
                return Directory.GetFiles(path, "*.SF3CHR")
                    .Concat(Directory.GetFiles(path, "*.SF3CHP"))
                    .OrderBy(x => x)
                    .ToArray();
            });
            if (files.Length == 0) {
                Logger.WriteLine("No file(s) or path(s) provided", LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Logger.WriteLine("Unrecognized arguments in 'depends' command: " + string.Join(" ", args), LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            if (verbose)
                Logger.WriteLine("Describing files:");
            var ngc = new NameGetterContext(ScenarioType.Scenario1);
            var currentDir = Environment.CurrentDirectory.Replace('\\', '/');
            using (Logger.IndentedSection(verbose ? 1 : 0)) {
                foreach (var file in files) {
                    var fileLower = file.ToLower();
                    try {
                        var depends = new List<string>();

                        if (fileLower.EndsWith(".sf3chr"))
                            depends.AddRange(GetDependsForSF3CHR(file, verbose, ngc));
                        else if (fileLower.EndsWith(".sf3chp"))
                            depends.AddRange(GetDependsForSF3CHP(file, verbose, ngc));
                        else {
                            Logger.WriteLine($"{file}: Not a valid compile target (.SF3CHR or .SF3CHP file)", LogType.Error);
                            continue;
                        }

                        var compiledFile = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)).Replace('\\', '/')
                            + (fileLower.EndsWith(".sf3chr") ? ".CHR" : ".CHP");

                        Logger.Write($"{compiledFile}: {file.Replace('\\', '/')}");
                        foreach (var depend in depends) {
                            var goodPath = depend.Replace('\\', '/');
                            if (goodPath.StartsWith(currentDir))
                                goodPath = goodPath.Substring(currentDir.Length + 1);
                            Logger.Write(" " + goodPath.Replace(" ", "\\ "));
                        }
                        Logger.FinishLine();
                    }
                    catch (Exception ex) {
                        Logger.Write($"{file}: ", LogType.Error);
                        Logger.LogException(ex);
                    }
                }
            }

            if (verbose)
                Logger.WriteLine("Done");

            return 0;
        }

        private static string[] GetDependsForSF3CHR(string file, bool verbose, INameGetterContext ngc) {
            return GetDependsForSF3CHR(CHR_Def.FromJSON(File.ReadAllText(file)), verbose, ngc);
        }

        private static string[] GetDependsForSF3CHR(CHR_Def sf3chrFile, bool verbose, INameGetterContext ngc) {
            var depends = new HashSet<string>();

            foreach (var sprite in sf3chrFile.Sprites) {
                if (sprite.FrameGroupsForSpritesheets != null) {
                    foreach (var fgfs in sprite.FrameGroupsForSpritesheets) {
                        if (fgfs.FrameGroups.Length > 0) {
                            var spriteName = fgfs.SpriteName ?? sprite.SpriteName;
                            depends.Add(SpriteResources.SpriteDefFile(spriteName));

                            var width = fgfs.Width ?? sprite.Width;
                            var height = fgfs.Height ?? sprite.Height;
                            if (width > 0 && height > 0)
                                depends.Add(SpriteResources.SpritesheetImageFile(spriteName, fgfs.Width ?? sprite.Width, fgfs.Height ?? sprite.Height));
                        }
                    }
                }
                if (sprite.AnimationsForSpritesheetAndDirections != null) {
                    foreach (var afsd in sprite.AnimationsForSpritesheetAndDirections) {
                        if (afsd.Animations.Length > 0) {
                            var spriteName = afsd.SpriteName ?? sprite.SpriteName;
                            depends.Add(SpriteResources.SpriteDefFile(spriteName));

                            var width = afsd.Width ?? sprite.Width;
                            var height = afsd.Height ?? sprite.Height;
                            if (width > 0 && height > 0)
                                depends.Add(SpriteResources.SpritesheetImageFile(spriteName, afsd.Width ?? sprite.Width, afsd.Height ?? sprite.Height));
                        }
                    }
                }
            }

            return depends.OrderBy(x => x).ToArray();
        }

        private static string[] GetDependsForSF3CHP(string file, bool verbose, INameGetterContext ngc) {
            var depends = new HashSet<string>();
            var sf3chpFile = CHP_Def.FromJSON(File.ReadAllText(file));
            foreach (var chr in sf3chpFile.CHRs) {
                var chrDepends = GetDependsForSF3CHR(chr, verbose, ngc);
                foreach (var depend in chrDepends)
                    depends.Add(depend);
            }
            return depends.OrderBy(x => x).ToArray();
        }
    }
}
