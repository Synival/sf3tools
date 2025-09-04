using SF3.CHR;
using SF3.Sprites;

namespace SpriteCHR_Builder {
    public class Program {
        public const string c_spritesheetPath = "../../../../SF3Lib/Resources/Spritesheets";
        public const string c_outputPath      = "../../../../SF3Lib/Resources/Rebuilt_CHRs/Sprites";

        public static int Main(string[] args) {
            // Load all sprite sheets ahead of time.
            // TODO: These really should be loaded on-demand.
            Console.WriteLine("Loading all SpriteDefs...");
            SpriteResources.SpritesheetPath = c_spritesheetPath;
            SpriteResources.LoadAllSpriteDefs();
            var spriteDefs = SpriteResources.GetAllLoadedSpriteDefs();

            Console.WriteLine("Writing CHRs...");
            _ = Directory.CreateDirectory(c_outputPath);

            var chrCompiler = new CHR_Compiler();

            foreach (var spriteDef in spriteDefs) {
                Console.WriteLine($"    {spriteDef.Name}");

                var chrJob = new CHR_CompilationJob();
                foreach (var spritesheetKv in spriteDef.Spritesheets) {
                    var spritesheetSize = Spritesheet.KeyToDimensions(spritesheetKv.Key);
                    var spritesheetDef = spritesheetKv.Value;

                    foreach (var animsWithDirectionsKv in spritesheetDef.AnimationSetsByDirections) {
                        var directions = animsWithDirectionsKv.Key;
                        var animations = animsWithDirectionsKv.Value;

                        // Split the animations into two sets:
                        // 1. A set of animations with all frames, and
                        // 2. A set of animations with at least one missing frame.
                        var animationsWithCompleteness = animations.AnimationsByName
                            .ToDictionary(x => x.Key, x => (Animation: x.Value, HasAllFrames: x.Value.HasAllFrames(directions)));

                        AnimationsForSpritesheetAndDirection[] completeAnimations = [
                            new AnimationsForSpritesheetAndDirection() {
                                Animations = animationsWithCompleteness
                                    .Where(x => x.Value.HasAllFrames)
                                    .Select(x => x.Key)
                                    .ToArray()
                            }
                        ];

                        var incompleteAnimations = animationsWithCompleteness
                            .Where(x => !x.Value.HasAllFrames)
                            .Select(x => new AnimationsForSpritesheetAndDirection() { Animations = [x.Key]})
                            .ToArray();

                        // Add all complete animations.
                        if (completeAnimations.Length > 0) {
                            chrJob.InitNewSprite(spritesheetDef, spritesheetSize.Width, spritesheetSize.Height, directions, 0);
                            chrJob.AddMissingFrames(completeAnimations, spriteDef.Name, spritesheetSize.Width, spritesheetSize.Height, directions);
                            chrJob.AddAnimations(completeAnimations, spriteDef.Name, spritesheetSize.Width, spritesheetSize.Height, directions);
                        }

                        // Add all incomplete animations as individual sprites. (If they were in the same sprite, the missing frames would be replaced with other frames!)
                        foreach (var animation in incompleteAnimations) {
                            chrJob.InitNewSprite(spritesheetDef, spritesheetSize.Width, spritesheetSize.Height, directions, 0);
                            chrJob.AddMissingFrames(animation, spriteDef.Name, spritesheetSize.Width, spritesheetSize.Height, directions);
                            chrJob.AddAnimations(animation, spriteDef.Name, spritesheetSize.Width, spritesheetSize.Height, directions);
                        }
                    }

                    // TODO: add a special sprite that contains only frames that weren't used.
                    // (Benetram has some of these, for example.)
                }

                using (var memoryStream = new MemoryStream()) {
                    chrJob.Write(memoryStream, false, null);
                    var chrOutPath = Path.Combine(c_outputPath, SpriteResources.FilesystemName(spriteDef.Name) + ".CHR");
                    File.WriteAllBytes(chrOutPath, memoryStream.ToArray());
                }
            }

            return 0;
        }
    }
}
