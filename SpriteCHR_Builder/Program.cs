using SF3.CHR;
using SF3.Sprites;
using SF3.Types;
using SF3.Utils;

namespace SpriteCHR_Builder {
    public class Program {
        public const string c_spritesheetPath = "../../../../SF3Lib/Resources/Spritesheets";
        public const string c_outputPath      = "../../../../SF3Lib/Resources/Rebuilt_CHRs/Sprites";

        public static int Main(string[] args) {
            // Load all sprite sheets ahead of time.
            // TODO: These really should be loaded on-demand.
            Console.WriteLine("Loading SpriteDefs...");
            SpriteUtils.SetSpritesheetPath(c_spritesheetPath);
            SpriteUtils.LoadAllSpriteDefs();
            var spriteDefs = SpriteUtils.GetAllSpriteDefs();

            Console.WriteLine("Writing CHRs...");
            _ = Directory.CreateDirectory(c_outputPath);

            foreach (var spriteDef in spriteDefs) {
                Console.WriteLine($"    {spriteDef.Name}");

                var chrSpriteDefs = new List<SF3.CHR.SpriteDef>();
                foreach (var spritesheetKv in spriteDef.Spritesheets) {
                    var spritesheetSize = SpritesheetDef.KeyToDimensions(spritesheetKv.Key);
                    var spritesheetDef = spritesheetKv.Value;

                    var allDirections = spritesheetDef.AnimationByDirections
                        .Select(x => x.Key)
                        .Distinct()
                        .ToArray();

                    foreach (var directions in allDirections) {
                        var animationsWithCompleteness = spritesheetDef.AnimationByDirections[directions].Animations
                            .ToDictionary(x => x.Key, x => (Animation: x.Value, HasAllFrames: x.Value.HasAllFrames(directions)));

                        SpriteAnimationsDef[] completeAnimations = [
                            new SpriteAnimationsDef() {
                                Animations = animationsWithCompleteness
                                    .Where(x => x.Value.HasAllFrames)
                                    .Select(x => x.Key)
                                    .ToArray()
                            }
                        ];

                        var incompleteAnimations = 
                            animationsWithCompleteness
                                .Where(x => !x.Value.HasAllFrames)
                                .Select(x => new SpriteAnimationsDef[] { new SpriteAnimationsDef() { Animations = [x.Key]}})
                                .ToArray();

                        SF3.CHR.SpriteDef NewChrSprite(SpriteAnimationsDef[] animations) {
                            // TODO: let the compiler add these automatically
                            var aniDefAnimations = animations
                                .SelectMany(x => x.Animations)
                                .Distinct()
                                .OrderBy(x => x)
                                .Select(x => spritesheetDef.AnimationByDirections[directions].Animations[x])
                                .ToArray();

                            // Gather all the frame groups required by every animation.
                            // TODO: Whatever is happening below definitely won't work long-term. The right solution is
                            // a bit tricky, so this will take some work.
                            var frames = aniDefAnimations
                                .SelectMany(x => {
                                    var currentDir = directions;
                                    return x.AnimationCommands
                                        .Select(y => {
                                            if (y.Command == SpriteAnimationFrameCommand.SetDirectionCount) {
                                                currentDir = y.Parameter;
                                                return null;
                                            }
                                            else if (y.Command == SpriteAnimationFrameCommand.Frame) {
                                                var expectedFrames = CHR_Utils.GetCHR_FrameGroupDirections(currentDir);
                                                var frames = (y.FrameGroup != null)
                                                    ? expectedFrames.ToDictionary(z => z, z => new AnimationFrameDirectionDef() { Frame = y.FrameGroup, Direction = z })
                                                    : y.Frames;

                                                // If there aren't enough frames, there are 'null's missing. (SF3Sprite error?)
                                                // Add them to the end.
                                                foreach (var ef in expectedFrames)
                                                    if (!frames.ContainsKey(ef))
                                                        frames.Add(ef, null!);

                                                return frames;
                                            }
                                            else
                                                return null;
                                        })
                                        .Where(x => x != null)
                                        // This two statements together will filter out all animation frames with nulls except for the one with the *most* frames.
                                        // This is fix to the bogus Rainblood Rook (Capeless) animation, which has missing frames in multiple animation frames.
                                        // Selecting the animation frame with the fewest missing frames will ensure that the other frames with missing frames exist.
                                        // (This really isn't always the case, it's just a hack for this one...)
                                        .OrderBy(x => x.Count(y => y.Value == null))
                                        .GroupBy(x => x.Count(y => y.Value == null) == 0)
                                        .SelectMany(x => x.Key == true ? x.ToArray() : [x.First()]);
                                })
                                .GroupBy(x => string.Join('|', x.Select(y => $"({y.Key}|{y.Value?.Frame}|{y.Value?.Direction})")))
                                .Select(x => x.First()!.Values)
                                .SelectMany(x => x
                                    .Where(y => y != null)
                                    .Select(y => new SF3.CHR.FrameGroupDef() {
                                        Name = y.Frame,
                                        Frames = [new SF3.CHR.FrameDef() { Direction = y.Direction }]
                                    })
                                )
                                .ToArray();

                            return new SF3.CHR.SpriteDef() {
                                SpriteID       = spritesheetDef.SpriteID,
                                SpriteName     = spriteDef.Name,
                                Width          = spritesheetSize.Width,
                                Height         = spritesheetSize.Height,
                                Directions     = directions,
                                PromotionLevel = 0,
                                VerticalOffset = spritesheetDef.VerticalOffset,
                                Unknown0x08    = spritesheetDef.Unknown0x08,
                                CollisionSize  = spritesheetDef.CollisionSize,
                                Scale          = spritesheetDef.Scale,

                                SpriteFrames   = [new SpriteFramesDef() { FrameGroups = frames }],
                                SpriteAnimations = animations
                            };
                        }

                        if (completeAnimations.Length > 0)
                            chrSpriteDefs.Add(NewChrSprite(completeAnimations));

                        foreach (var animation in incompleteAnimations)
                            chrSpriteDefs.Add(NewChrSprite(animation));
                    }

                    // TODO: add a special sprite that contains only frames that weren't used.
                    // (Benetram has some of these, for example.)
                }

                var chrDef = new CHR_Def() {
                    Sprites = chrSpriteDefs.ToArray()
                };

                using (var memoryStream = new MemoryStream()) {
                    chrDef.ToCHR_File(memoryStream);
                    var chrOutPath = Path.Combine(c_outputPath, SpriteUtils.FilesystemName(spriteDef.Name) + ".CHR");
                    File.WriteAllBytes(chrOutPath, memoryStream.ToArray());
                }
            }

            return 0;
        }
    }
}
