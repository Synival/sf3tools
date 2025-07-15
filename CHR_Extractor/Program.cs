using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using Newtonsoft.Json;
using SF3;
using SF3.ByteData;
using SF3.Models.Files;
using SF3.Models.Files.CHP;
using SF3.Models.Files.CHR;
using SF3.Models.Structs.CHR;
using SF3.NamedValues;
using SF3.Sprites;
using SF3.Types;
using SF3.Utils;

namespace CHR_Analyzer {
    public class Program {
        // ,--- Enter the paths for all your CHR files here!
        // v
        private static readonly Dictionary<ScenarioType, string> c_pathsIn = new() {
            { ScenarioType.Scenario1,   "D:/" },
            { ScenarioType.Scenario2,   "E:/" },
            { ScenarioType.Scenario3,   "F:/" },
            { ScenarioType.PremiumDisk, "G:/" },
        };

        private const string c_pathOut = "../../../Private";

        private class TextureInfo {
            public TextureInfo(UniqueFrameDef frameInfo, ITexture texture) {
                FrameInfo = frameInfo;
                Texture = texture;
            }

            public override string ToString() => FrameInfo.ToString();

            public UniqueFrameDef FrameInfo { get; }
            public ITexture Texture { get; }
            public List<TextureSpriteInfo> Sprites { get; } = new List<TextureSpriteInfo>();
        }

        private class TextureSpriteInfo {
            public TextureSpriteInfo(ScenarioType scenario, string filename, int spriteId) {
                Scenario = scenario;
                Filename = filename;
                SpriteID = spriteId;
            }

            public readonly ScenarioType Scenario;
            public readonly string Filename;
            public readonly int SpriteID;
        }

        private class AnimationInfo {
            public AnimationInfo(UniqueAnimationDef animationInfo) {
                AnimInfo = animationInfo;
            }

            public override string ToString() => AnimInfo.ToString();

            public UniqueAnimationDef AnimInfo { get; }
            public List<AnimationFileSprite> Sprites { get; } = new List<AnimationFileSprite>();
        }

        private class AnimationFileSprite {
            public AnimationFileSprite(ScenarioType scenario, string filename, int spriteIndex, int animIndex, int lastFrameWord) {
                Scenario      = scenario;
                Filename      = filename;
                SpriteIndex   = spriteIndex;
                AnimIndex     = animIndex;
                LastFrameWord = lastFrameWord;
            }

            public readonly ScenarioType Scenario;
            public readonly string Filename;
            public readonly int SpriteIndex;
            public readonly int AnimIndex;
            public readonly int LastFrameWord;
        }

        private static Dictionary<string, TextureInfo> s_framesByHash = [];
        private static Dictionary<string, AnimationInfo> s_animationsByHash = [];

        // TODO: remove and just fetch the serializable data from SF3Lib. Remove all related methods and classes.
        private static void AddFrame(ScenarioType scenario, string filename, int spriteId, Frame frame) {
            var hash = frame.Texture.Hash;
            if (!s_framesByHash.ContainsKey(hash))
                s_framesByHash.Add(hash, new TextureInfo(frame.FrameInfo, frame.Texture));
            s_framesByHash[hash].Sprites.Add(new TextureSpriteInfo(scenario, filename, spriteId));
        }

        // TODO: remove and just fetch the serializable data from SF3Lib. Remove all related methods and classes.
        private static void AddAnimation(ScenarioType scenario, string filename, int spriteIndex, Animation animation) {
            var hash = animation.Hash;
            if (!s_animationsByHash.ContainsKey(hash))
                s_animationsByHash.Add(hash, new AnimationInfo(animation.AnimationInfo));

            var lastFrame = animation.AnimationFrames.LastOrDefault();
            var lastFrameWord = (lastFrame == null) ? 0 : (lastFrame.FrameID << 8) | lastFrame.Duration;

            s_animationsByHash[hash].Sprites.Add(new AnimationFileSprite(scenario, filename, spriteIndex, animation.ID, lastFrameWord));
        }

        public static void Main(string[] args) {
            Console.WriteLine("Processing all CHR and CHP files...");

            // Get a list of all .MPD files from all scenarios located at 'c_pathsIn[Scenario]'.
            var allFiles = Enum.GetValues<ScenarioType>()
                .Where(x => c_pathsIn.ContainsKey(x))
                .ToDictionary(x => x, x => {
                    var files = Directory.GetFiles(c_pathsIn[x], "*.CHR").Order().ToList();
                    files.AddRange(Directory.GetFiles(c_pathsIn[x], "*.CHP").Order().ToList());
                    return files;
                });

            var nameGetterContexts = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            // Open each file.
            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];

                foreach (var file in filesKv.Value) {
                    var filename = Path.GetFileNameWithoutExtension(file);

                    // Get a byte data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Create a CHR file that works with our new ByteData.
                    try {
                        bool isChr = file.EndsWith(".CHR");
                        using (ScenarioTableFile chrChpFile = isChr
                            ? CHR_File.Create(byteData, nameGetterContexts[scenario], scenario)
                            : CHP_File.Create(byteData, nameGetterContexts[scenario], scenario)
                        ) {
                            var chrFiles = isChr
                                ? [(CHR_File) chrChpFile]
                                : ((CHP_File) chrChpFile).CHR_EntriesByOffset.Values.ToArray();

                            // List the file and any report we may have from CHR_MatchFunc().
                            var fileStr = GetFileString(scenario, file, chrChpFile);
                            Console.WriteLine($"{fileStr}");

                            // Build a table of all textures.
                            foreach (var sprite in chrFiles.SelectMany(x => x.SpriteTable).ToArray()) {
                                foreach (var frame in sprite.FrameTable)
                                    AddFrame(scenario, filename, sprite.ID, frame);
                                foreach (var animation in sprite.AnimationTable)
                                    AddAnimation(scenario, filename, sprite.ID, animation);
                            }
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("  !!! Exception for '" + filename + "': '" + e.Message + "'. Skipping!");
                    }
                }
            }

            Console.WriteLine("Processing complete.");
            _ = Directory.CreateDirectory(c_pathOut);

            Console.WriteLine();
            Console.WriteLine("===================================================");
            Console.WriteLine("| NAMING FRAMES                                   |");
            Console.WriteLine("===================================================");
            Console.WriteLine();

            var animFrameNamingPriorityByCategory = new Dictionary<string, string[]>() {
                // ----------------------------------
                //  Idle (Specific for Field/Battle)
                // ----------------------------------
                { "Idle (Field)", [
                    "Idle (Field)",
                    "Idle (Field, Should be 4 Directions)",
                ]},
                { "Idle (Field, Leaning Back)", [
                    "Idle (Field, Leaning Back)",
                ]},
                { "Idle (Battle)", [
                    "Idle (Battle)",
                    "Idle (Battle 1)",
                    "Idle (Battle 2)",
                    "Idle (Battle, Render 1)",
                    "Idle (Battle, Render 2)",
                    "Idle (Battle, Missing Last Down Frame)",
                ]},
                { "Idle (Upright)", [
                    "Idle (Upright, Some Frames Offset)",
                ]},
                { "Idle", [
                    "Idle",
                    "Idle 1",
                    "Idle 2",
                    "Idle (Render 1)",
                    "Idle (Render 2)",
                    "Idle (Slower)",
                    "Idle (Fast)",
                    "Idle (Faster)",
                    "Idle (Steady)",
                    "Idle (Pause at Top)",
                    "Idle (ShakingFist)",
                    "Idle (LiftingArms)",
                    "Idle (2 Frames)",
                    "Idle (1 Direction, Facing NNE)",
                ]},
                { "Idle (Fixed Height)", ["Idle (Fixed Height)"] },
                { "Idle (Wrong Height)", ["Idle (Wrong Height)"] },

                // ---------------------------
                //  Idle
                // ---------------------------
                { "Idle (Very Still)", ["Idle (Very Still)"]},
                { "Idle (Tussling Hair)", ["Idle (Tussling Hair)"]},
                { "Idle (Imposter?)", ["Idle (Imposter?)"]},
                { "Idle (Tapping Foot)", ["Idle (Tapping Foot)"]},
                { "Idle (Tipping)", ["Idle (Tipping)"]},
                { "Idle (Arms Crossed)", ["Idle (Arms Crossed)"]},
                { "Idle (Rocking Baby)", ["Idle (Rocking Baby)"]},
                { "Idle (Wind Blowing Hair)", ["Idle (Wind Blowing Hair)"]},
                { "Idle (Winking)", ["Idle (Winking)"]},
                { "Idle (Occasionally Raising Hand)", ["Idle (Occasionally Raising Hand)"]},
                { "Idle (Occasionally Wipes Forehead)", ["Idle (Occasionally Wipes Forehead)"]},
                { "Idle (Posing)", ["Idle (Posing)"]},
                { "Idle (Snapping)", ["Idle (Snapping)"]},
                { "Idle (Bobbing)", ["Idle (Bobbing)"]},
                { "Idle (Grazing)", ["Idle (Grazing)"]},
                { "Idle (Chewing)", ["Idle (Chewing)"]},
                { "Idle (Roaring)", ["Idle (Roaring)"]},
                { "Idle (Fish Quivering)", ["Idle (Fish Quivering)"]},
                { "Idle (Still then Nibbling)", ["Idle (Still then Nibbling)"]},
                { "Idle (Occasionally Looking Around)", ["Idle (Occasionally Looking Around)"]},
                { "Idle (Hovering)", [
                    "StillFrame (Hovering)",
                    "StillFrame (Hovering, Change to 0)",
                    "StillFrame (Hovering, Repeat)",
                ]},
                { "Idle (Resting)", [
                    "Idle (Resting)",
                    "Resting (Absurdly Long Duration)",
                    "Resting (Occassionally Scratches)",
                    "Resting (Very Subtle Movement)",
                    "StillFrame (Resting)",
                    "StillFrame (Resting, Repeat)",
                ]},
                { "Idle (Sitting)", ["Idle (Sitting)"]},
                { "Idle (Kneeling)", ["Idle (Kneeling)"]},
                { "Idle (Dangling)", ["Idle (Dangling)"]},
                { "Idle (KeeledOver)", ["Idle (KeeledOver)"]},
                { "Idle (SittingInChair)", [
                    "StillFrame (SittingInChair)",
                    "StillFrame (SittingInChair, Repeat)",
                ]},
                { "OnGround", [
                    "StillFrame (OnGround)",
                    "StillFrame (OnGround, Repeat)",
                    "StillFrame (OnGround, Repeat 1)",
                    "StillFrame (OnGround, Repeat 2)",
                    "StillFrame (OnGround 1)",
                    "StillFrame (OnGround 1, Repeat)",
                ]},
                { "OnGround (Alt)", [
                    "StillFrame (OnGround 2)",
                    "StillFrame (OnGround 2, Repeat)",
                ]},

                // ---------------------------
                //  Walking or Similar
                // ---------------------------
                { "Walking", [
                    "Walking",
                    "Walking (Render 1)",
                    "Walking (Render 2)",
                    "Walking (Faster)",
                    "Walking (Slower)",
                    "Walking (Very Fast)",
                    "Walking (Reduced)",
                    "Walking (Slow)",
                    "Walking (Fast)",
                    "Walking (1 Direction)",
                    "Walking (Bad ENE)",
                    "Walking (Bad ESE Offset)",
                    "Walking (NESW, Faster)",
                    "Walking (NESW, Slower)",
                    "Walking (Should be 1 Direction)",
                    "Walking (Should be 4 Directions)",
                ]},
                { "Walking (Fan Down)", ["Walking (Fan Down)"]},
                { "Running", ["Running"]},
                { "Flying (Field)", [
                    "Flying (Field)",
                ]},
                { "Flying (Battle)", [
                    "Flying (Battle)",
                    "Flying (Battle, Faster)",
                    "Flying (Battle, Slower)",
                ]},
                { "Flying", [
                    "Flying",
                    "Flying (Normal)",
                    "Flying (Fast)",
                    "Flying (Faster)",
                    "Flying (Medium)",
                    "Flying (Slower)",
                    "Flying (Slow)",
                    "Flying (Slow 1)",
                    "Flying (Slow 2)",
                    "Flying (Slower 1)",
                    "Flying (Slower 2)",
                    "Flying (Very Fast)",
                    "Flying (Flapping Quickly)",
                ]},
                { "Flying (Bad)", [
                    "Flying (Fast, Reduced)",
                    "Flying (Bad Outline, Reduced, Redundant Frames)",
                ]},
                { "Hovering", [
                    "Hovering",
                    "Hovering (Fast)",
                    "Hovering (Faster)",
                    "Hovering (Slower)",
                    "Hovering (Slow)",
                ]},
                { "WalkingWithBox", ["WalkingWithBox"]},
                { "Walking (Flowers Behind Back)", ["Walking (Flowers Behind Back)"]},
                { "Walking (Hopping)", ["Walking (Hopping)"]},
                { "Walking (Sideways)", ["Walking (Sideways)"]},
                { "Moving", [
                    "Moving",
                    "Moving (Fast)",
                    "Moving (Slow)",
                ]},

                // ---------------------------
                //  Nodding
                // ---------------------------
                { "Nodding (Field)", [
                    "Nodding (Field)",
                    "Nodding (Field, Short 2)",
                    "Nodding (Field, Incomplete)",
                    "Nodding (From Walking to Field)",
                    "Nodding (From Battle to Field)",
                    "Nodding (Quick, From Fan Down to Field)",
                    "Nodding (Wrong First Frame)",
                ]},
                { "Nodding (Battle)", [
                    "Nodding (Battle)",
                    "Nodding (Battle, Short 1)",
                    "Nodding (Battle, Reduced)",
                    "Nodding (Battle, Incomplete)",
                    "Nodding (From Field to Battle, Very Quick)",
                ]},
                { "Nodding", [
                    "Nodding",
                    "Nodding (Incomplete, Missing End)",
                    "Nodding (Should be 4 Directions)",
                    "Nodding 1",
                    "Nodding 2",
                    "Nodding (Slow)",
                    "Nodding (Slower)",
                    "Nodding (Slower, Stop)",
                    "Nodding (Repeat)",
                    "Nodding (Change to 0)",
                    "Nodding (Change to 1)",
                    "Nodding (Stop)",
                    "Nodding (Stop, Change to 0)",
                    "Nodding (Stop, Change to 1)",
                    "Nodding (Faster, Stop)",
                    "Nodding (Fast)",
                    "Nodding (Fast, Idle)",
                    "Nodding (Fast, Stop)",
                    "Nodding (Walking)",
                    "Nodding (Walking, Very Quick)",
                    "Nodding (Walking, Short)",
                    "Nodding (Some Frames Offset)",
                    "Nodding (First Frame NESW)",
                    "Nodding (Weird Angle)",
                    "Nodding (Head Down)",
                    "Nodding (Fast, Walking)",
                    "Nodding (Short)",
                    "Nodding (Several, Quick)",
                    "Nodding (Twice, Quick 1)",
                    "Nodding (Twice, Quick 2)",
                    "Nodding (Reduced, Double, First Frame Cape High)",
                    "Nodding (Reduced, Double, First Frame Cape Low)",
                    "Nodding (Sitting in Chair, Changes to 1 Direction)",
                ]},
                { "Nodding (Fan Down)", [
                    "Nodding (Quick, Fan Down)",
                ]},
                { "Nodding (Kneeling)", [
                    "Nodding (Kneeling)",
                    "Nodding (Kneeling, NoStop)",
                ]},
                { "Nodding (Sitting in Chair)", [
                    "Nodding (Sitting in Chair)",
                ]},
                { "Nodding (OnGround)", [
                    "Nodding (OnGround)",
                ]},

                // ---------------------------
                //  Shaking Head
                // ---------------------------
                { "ShakingHead (Field)", [
                    "ShakingHead (Field)",
                    "ShakingHead (Field, Very Quick)",
                    "ShakingHead (Field, Weird Angle)",
                    "ShakingHead (Field, Weird Angle, Short)",
                    "ShakingHead (Field, Skips Frame)",
                    "ShakingHead (From Walking to Field)",
                    "ShakingHead (From Battle to Field)",
                ]},
                { "ShakingHead (Battle)", [
                    "ShakingHead (Battle)",
                    "ShakingHead (Battle, Looking Down)",
                ]},
                { "ShakingHead", [
                    "ShakingHead",
                    "ShakingHead (Incomplete, Missing End)",
                    "ShakingHead (Should be 4 Directions)",
                    "ShakingHead (Some Frames Offset)",
                    "ShakingHead (Repeat)",
                    "ShakingHead (Stop)",
                    "ShakingHead (Stop, Change to 0)",
                    "ShakingHead (Stop, Change to 1)",
                    "ShakingHead (Change to 0)",
                    "ShakingHead (Change to 1)",
                    "ShakingHead (Slow)",
                    "ShakingHead (Once)",
                    "ShakingHead (Fast)",
                    "ShakingHead (Quick)",
                    "ShakingHead (Faster)",
                    "ShakingHead (Super Fast, Idle)",
                    "ShakingHead (Crazy Fast)",
                    "ShakingHead (Weird Angle)",
                    "ShakingHead (Fast, Weird, Walking with Idle Frame)",
                    "ShakingHead (Head Down)",
                    "ShakingHead (Leaning Forward)",
                    "ShakingHead (Slanted, Fast)",
                    "ShakingHead (To the Left, Stops)",
                ]},
                { "ShakingHead (Fan Down)", [
                    "ShakingHead (Fan Down)",
                ]},
                { "ShakingHead (Kneeling)", [
                    "ShakingHead (Kneeling)",
                    "ShakingHead (Kneeling, NoStop)",
                ]},
                { "ShakingHead (On Knees)", [
                    "ShakingHead (On Knees)",
                ]},
                { "ShakingHead (Sitting in Chair)", [
                    "ShakingHead (Sitting in Chair)",
                    "ShakingHead (Sitting in Chair, Janky)",
                ]},
                { "ShakingHead (Sitting)", [
                    "ShakingHead (Sitting)",
                ]},
                { "ShakingHead (Sitting Cross-Legged)", [
                    "ShakingHead (Sitting Cross-Legged)",
                ]},
                { "ShakingHead (OnGround)", [
                    "ShakingHead (OnGround)",
                ]},

                // ---------------------------
                //  Actions (Standing)
                // ---------------------------
                { "ShakingEntireBody", [
                    "ShakingHead (WholeBody)",
                    "ShakingHead (MovingWholeBody)",
                    "ShakingEntireBody (Strange, Has Carried Away Frame)",
                ] },
                { "GivingThumbsUp", [
                    "GivingThumbsUp (ENE)",
                    "GivingThumbsUp (NNE)",
                ]},
                { "BreathingFire", [
                    "BreathingFire1",
                    "BreathingFire2",
                    "BreathingFire3",
                    "BreathingFire4",
                    "BreathingFire5",
                ]},
                { "Casting", [
                    "Casting",
                    "StillFrame (Casting)",
                ]},
                { "Attacking", [
                    "Attacking (1 Direction)",
                    "Attacking (Reduced, 1 Direction)",
                ]},
                { "Drinking", [
                    "Drinking"
                ]},
                { "Pointing", [
                    "Pointing",
                    "Pointing (Slow)",
                    "Pointing (Fast)",
                    "Pointing (1 Direction)",
                    "StopPointing (Should be 1 Direction)",
                ]},
                { "Panting", [
                    "Panting (1 Direction)",
                    "StillFrame (Panting, Should be 1 Direction)",
                ]},
                { "AdjustGlasses", [
                    "AdjustGlasses (Should be 4 Directions)",
                ]},
                { "ShakingFist", ["ShakingFist"] },
                { "StartOrStopShakingFist", [
                    "StartShakingFist",
                    "StopShakingFist (Changes to Nodding)",
                ] },
                { "Shushing", ["Shushing"] },
                { "StartOrStopShushing", ["ShushingBackToIdle"] },
                { "AimingCannonUpwards", ["AimingCannonUpwards"] },
                { "Bathing", ["Bathing"] },
                { "Bobbing", ["Bobbing"] },
                { "BobbingSideToSide", ["BobbingSideToSide"] },
                { "HoppingAround", ["HoppingAround"] },
                { "CarriedAwayByBirdMen", ["CarriedAwayByBirdMen"] },
                { "Clapping?", ["Clapping?"] },
                { "Crying", ["Crying"] },
                { "CuteLittleKick", ["CuteLittleKick"] },
                { "CuttingRope", ["CuttingRope"] },
                { "FinishingJumpKick", ["FinishingJumpKick"] },
                { "Frustrated", ["Frustrated"] },
                { "Giggle", ["Giggle"] },
                { "Hammering", ["Hammering"] },
                { "HoldingOutArms", ["HoldingOutArms"] },
                { "HoldingOutHand", ["HoldingOutHand"] },
                { "HurlingBoulder", ["HurlingBoulder"] },
                { "InDespair", ["InDespair"] },
                { "JumpingUp", ["JumpingUp"] },
                { "Kissing", ["Kissing"] },
                { "Kissing (Blushing)", ["Kissing (Blushing)"] },
                { "Landing", ["Landing"] },
                { "Laughing", ["Laughing"] },
                { "LeaningAgainstWall", ["LeaningAgainstWall"] },
                { "LeapingToDad", ["LeapingToDad"] },
                { "LiftingBox", ["LiftingBox"] },
                { "LiftingUpFlareGun", ["LiftingUpFlareGun"] },
                { "LiftingWeight", ["LiftingWeight"] },
                { "Limping", ["Limping"] },
                { "LookingUp", ["LookingUp"] },
                { "LoweringCannonSlowly", ["LoweringCannonSlowly"] },
                { "MoveDownUpWhileFeyYawns", ["MoveDownUpWhileFeyYawns"] },
                { "MovingRightWhileFeySlaps", ["MovingRightWhileFeySlaps"] },
                { "OccasionallyDabbingFace", ["OccasionallyDabbingFace"] },
                { "OccasionallyLookingUp", ["OccasionallyLookingUp"] },
                { "OccasionallyTusslingHair", ["OccasionallyTusslingHair"] },
                { "OccasionallyWipingForehead", ["OccasionallyWipingForehead"] },
                { "OneHandedPushUps", ["OneHandedPushUps"] },
                { "OpeningEyes (OnGround)", ["OpeningEyes (OnGround)"] },
                { "OrbHovering", ["OrbHovering"] },
                { "PantingOnGround", ["PantingOnGround"] },
                { "PeekingThroughWindow", ["PeekingThroughWindow"] },
                { "PointingToCrystal", ["PointingToCrystal"] },
                { "Popping", ["Popping"] },
                { "PoundingGround", ["PoundingGround"] },
                { "PresentingFlowers", ["PresentingFlowers"] },
                { "Pulsating", ["Pulsating"] },
                { "PuttingBoxDown", ["PuttingBoxDown"] },
                { "RaisingCane", ["RaisingCane"] },
                { "RaisingHands", ["RaisingHands"] },
                { "RaisingStaff", ["RaisingStaff"] },
                { "RisingUp", ["RisingUp"] },
                { "RollingBackward", ["RollingBackward"] },
                { "Saluting", ["Saluting"] },
                { "ShakingWithRage", ["ShakingWithRage"] },
                { "ShieldingSelf", ["ShieldingSelf"] },
                { "Signalling", ["Signalling"] },
                { "SlapIshahakat", [
                    "SlapIshahakat",
                    "SlapIshahakat (Long, Change to 1)",
                    "SlapIshahakat (Stop)",
                ] },
                { "SleepingWhileStanding", ["SleepingWhileStanding"] },
                { "SlowlyGettingUpFromGround", ["SlowlyGettingUpFromGround"] },
                { "SomersaultToStillMidAirPose", ["SomersaultToStillMidAirPose"] },
                { "SpinUpToDown", ["SpinUpToDown"] },
                { "SpinningUpToDown", ["SpinningUpToDown"] },
                { "Splash", ["Splash"] },
                { "StartingJumpKick", ["StartingJumpKick"] },
                { "StopCrying", ["StopCrying"] },
                { "Sweeping", ["Sweeping"] },
                { "TappingChin?", ["TappingChin?"] },
                { "ThrowingBomb", ["ThrowingBomb"] },
                { "TouchingIshahakat", ["TouchingIshahakat"] },
                { "TurningBack", ["TurningBack"] },
                { "TurningTranslucent", ["TurningTranslucent"] },
                { "UsingStaff", ["UsingStaff"] },
                { "Waving", ["Waving"] },
                { "WigglingOnGround", ["WigglingOnGround"] },
                { "Yawning", ["Yawning"] },
                { "SittingAndEating", [
                    "SittingAndEating",
                ]},
                { "Backflip", [
                    "Backflip",
                    "Backflip (Slow, Repeat)",
                    "Backflip (Fast, 2x, Change to 0)",
                ]},
                { "Somersault", [
                    "Somersault",
                ]},
                { "GettingZapped", [
                    "GettingZapped (Slow)",
                    "GettingZapped (Fast)",
                ]},
                { "Glowing", [
                    "Glowing",
                    "Glowing (1 Direction)",
                    "Glowing (1 Direction, Stop)",
                ]},
                { "HoldingUpStaff", ["HoldingUpStaff"] },
                { "HoldingUpCane", ["HoldingUpCane"] },
                { "HoldingUpFan", ["HoldingUpFan"] },
                { "RecitingScripture", [
                    "RecitingScripture (1 Direction)",
                    "DoneRecitingScripture (1 Direction)",
                ]},
                { "HoldingUpHand", [
                    "HoldingUpHand (1 Direction)",
                ]},
                { "LeaningDownAndWaving", [
                    "LeaningDownAndWaving",
                    "LeaningDownAndWaving (Repeat)",
                ]},
                { "LeaningOnSword", [
                    "LeaningOnSword (Slow)",
                    "LeaningOnSword (Fast)",
                    "StillFrame (LeaningOnSword)",
                ]},
                { "LiftingHands", [
                    "LiftingHands (1 Direction)",
                ]},
                { "GlowingWithRaisedHands", [
                    "LiftingHands (Then Glowing)",
                ]},
                { "LookingAround (Sitting)", ["LookingAround (Sitting)"]},
                { "LookingLeft (Kneeling)", ["LookingLeft (Kneeling)", "LookingForwardFromLeft (Kneeling)"]},
                { "LookingRight (Kneeling)", ["LookingRight (Kneeling)", "LookingForwardFromRight (Kneeling)"]},
                { "LookingAtUs (Kneeling)", ["LookingAtUs (Kneeling, 1 Direction)", "NoLongerLookingAtUs (Kneeling, 1 Direction)"]},
                { "LookingBack", [
                    "LookingBack",
                    "LookingBackForward",
                    "LookingBack (1 Direction)",
                    "LookingBackForward (1 Direction)",
                ]},
                { "LookingDown", [
                    "LookingDown (Slow)",
                    "LookingDown (Fast)",
                ]},
                { "PullingOutLance", ["PullingOutLance (1 Direction)"] },
                { "PullingOutSword", ["PullingOutSword (1 Direction)"] },
                { "PullingOutTelescope", ["PullingOutTelescope (1 Direction)"] },
                { "RaisingArms", [
                    "RaisingArms (Duration 15)",
                    "RaisingArms (Duration 14)",
                ] },
                { "Shoveling", [
                    "Shoveling (Repeating Pattern)",
                    "Shoveling (Repeating Single)",
                ] },
                { "Wincing", [
                    "Wincing (Extremely Long Pause)",
                    "Wincing (Very Long Pause)",
                ] },
                { "Stretching", [
                    "Stretching",
                    "Stretching (Repeat)",
                    "Stretching (Change to 0)",
                ] },
                { "SpreadingOutArms", [
                    "SpreadingOutArms",
                    "SpreadingOutArms (Forever)",
                ] },

                // ---------------------------
                //  Actions on Ground
                // ---------------------------
                { "Sleeping", [
                    "Sleeping",
                    "Sleeping (Slow Snoring)",
                    "Sleeping (Slow Breathing)",
                    "Sleeping (Very Slow Breathing)",
                    "Sleeping (Fast Snoring)",
                ] },
                { "Twitching (OnGround)", ["Twitching (OnGround)"] },
                { "LoweringStaff (OnGround)", ["LoweringStaff (OnGround)"] },

                // ---------------------------
                //  Actions (Other Positions)
                // ---------------------------
                { "KneelingAndPraying", [
                    "KneelingAndPraying",
                    "KneelingAndPraying (Repeats after a while)",
                ]},
                { "LookingUp (Kneeling)", ["LookingUp (Kneeling)"] },

                // ---------------------------
                //  Changing Positions
                // ---------------------------
                { "GettingUpFromGround", ["GettingUpFromGround"] },
                { "GettingUpFromGround (Looks Silly)", ["GettingUpFromGround (Looks Silly)"] },
                { "GettingUp", ["GettingUp (Slowly)"] },
                { "GettingBackUp", ["GettingBackUp", "GettingBackUpSlowly"] },
                { "GettingKnockedOut", ["GettingKnockedOut"] },
                { "GettingUpFromKneeling", ["GettingUpFromKneeling"] },
                { "GettingUpFromKeeledOver", ["GettingUpFromKeeledOver"] },
                { "GettingUpFromPantingOnGround", ["GettingUpFromPantingOnGround"] },
                { "GettingUpFromPrayer", ["GettingUpFromPrayer"] },
                { "GoingBackDown", ["GoingBackDown (Slowly)"] },
                { "Kneeling", [
                    "Kneeling",
                    "Kneeling (1 Direction)",
                    "StillFrame (Kneeling 1, 1 Direction)",
                    "StillFrame (Kneeling 2, 1 Direction)",
                ]},
                { "Collapsing", [
                    "Collapsing",
                ]},
                { "SittingDownOrUpWithChair", [
                    "SittingDownIntoChair",
                    "SittingInChair",
                    "StandingUpFromChair",
                    "StandingUpFromChair (Slow)",
                    "StandingUpFromChair (Fast, Back into Chair?)",
                ]},
                { "StandingUpFromKneeling", ["StandingUpFromKneeling"] },
                { "StandingUpFromSitting", ["StandingUpFromSitting"] },
                { "StandingUpFromSittingCrossLegged", ["StandingUpFromSittingCrossLegged", "StandingUpFromSittingCrossLegged (Quick)"] },
                { "StandingUprightFromLeaningForward", ["StandingUprightFromLeaningForward"] },
                { "CollapsingFromPanting", [
                    "CollapsingFromPanting (1 Direction)",
                    "PantingFromCollapsed (1 Direction)",
                ]},
                { "KeelingOverFromSittingCrossLegged", ["KeelingOverFromSittingCrossLegged"] },

                // ---------------------------
                //  Object Animations
                // ---------------------------
                { "Exploding", [
                    "Exploding (w/ Transparent Frame)",
                    "Exploding (w/o Transparent Frame)",
                    "Exploding (Slow then Fast)",
                    "Exploding (Very Fast)",
                ]},
                { "Exploding (Alt)", [
                    "Exploding (Alt, w/o Transparent Frame)",
                ]},
                { "Unexploding", [
                    "Unexploding (w/ Transparent Frame)",
                    "Unexploding (w/o Transparent Frame)",
                ]},
                { "Unexploding (Alt)", [
                    "Unexploding (Alt, w/o Transparent Frame)",
                ]},
                { "Flickering", [
                    "Flickering",
                ]},
                { "Flickering (Small)", [
                    "Flickering (Small)",
                ]},
                { "Flickering (Large)", [
                    "Flickering (Large)",
                ]},
                { "Zapping", [
                    "Zapping (Slow)",
                    "Zapping (Fast)",
                    "Zapping (Reduced, Slow)",
                ]},
                { "Zapping (To the Left)", [
                    "Zapping (Fast, To the Left)",
                ]},
                { "Poofing", [
                    "Poofing",
                    "Poofing (Slow, Slowing Down)",
                    "Poofing (Fast, Slowing Down)",
                    "Poofing (Very Fast)",
                    "Poofing (Extremely Slow)",
                    "SinglePoof",
                ]},
                { "Shimmering", [
                    "Shimmering (Very Slow)",
                    "Shimmering (Fast)",
                ]},
                { "OpeningAndClosing", [
                    "Opening",
                    "Closing",
                ]},
                { "BlinkRedOnce", ["BlinkRedOnce"] },
                { "BlinkWhiteOnce", ["BlinkWhiteOnce"] },
                { "BlinkYellowOnce", ["BlinkYellowOnce"] },

                // ---------------------------
                //  Object Animations (Icons)
                // ---------------------------
                { "Alert", ["Alert"] },
                { "Angry", ["Angry"] },
                { "Complaining", ["Complaining"] },
                { "Confused", ["Confused"] },
                { "Embarassed", ["Embarassed"] },
                { "Flag", ["Flag"] },
                { "Idea", ["Idea"] },
                { "Love", ["Love"] },
                { "Shield", ["Shield"] },
                { "Silent", ["Silent"] },
                { "Sweat", ["Sweat (Growing Larger)", "Sweat"] },
                { "Sword", ["Sword"] },

                // ---------------------------
                //  Combo Animations
                // ---------------------------
                { "LookingBackThenToIdle", ["LookingBackThenToIdle"] },
                { "ShakingHeadAndFrustrated", ["ShakingHeadAndFrustrated"] },
                { "Blushing, BobbingSideToSide", ["Blushing, BobbingSideToSide"] },
                { "Hopping, Blushing, BobbingSideToSide", ["Hopping, Blushing, BobbingSideToSide"] },
                { "RaisingFanAndPointing", ["RaisingFanAndPointing (1 Direction)"] },
                { "IdleNoddingShakingHead", ["IdleNoddingShakingHead"] },

                // ---------------------------
                //  Bad Animation Group
                // ---------------------------
                { "Idle (Bad)", [
                    "Idle (Bad NNE Offset)",
                    "Idle (Faster, Bad Offset)",
                    "Idle (Offset)",
                    "Idle (Reduced, Janky)",
                ]},
                { "Nodding (Bad)", [
                    "Nodding (Offset)",
                    "Nodding (Field, Misaligned)",
                    "Nodding (Stops, Bad NNE Offsets)",
                ]},
                { "ShakingHead (Bad)", [
                    "ShakingHead (Full, Janky, Stops)",
                    "ShakingHead (Stops, Bad NNE Offsets)",
                    "ShakingHead (Fast, Bad Offsets)",
                    "ShakingHead (Fast, Bad Offsets, Stop)",
                    "ShakingHead (Misaligned)",
                ]},
                { "Pointing (Bad Offsets)", [
                    "Pointing (Offset)",
                ]},

                // ---------------------------
                //  Still Frames
                // ---------------------------
                { "Alt", [
                    "StillFrame (Alt)",
                ]},
                { "Black", [
                    "StillFrame (Black)",
                    "StillFrame (Black, Repeat)",
                ]},
                { "Black+White", [
                    "StillFrame (Black+White)",
                    "StillFrame (Black+White, Change to 0)",
                    "StillFrame (Black+White, Repeat, Duration 0)",
                ]},
                { "BlownBack", [
                    "StillFrame (Blown Back)",
                    "StillFrame (Blown Back, Repeat)",
                ]},
                { "Bomb", [
                    "StillFrame (Bomb)",
                ]},
                { "CannonBall", [
                    "StillFrame (CannonBall)",
                    "StillFrame (CannonBall, Change to 1)",
                    "StillFrame (CannonBall, Repeat)",
                ]},
                { "Cowering", [
                    "StillFrame (Cowering, 1 Direction)",
                ]},
                { "Deactivated", [
                    "StillFrame (Deactivated)",
                    "StillFrame (Deactivated, Repeat)",
                ]},
                { "Dot", [
                    "StillFrame (Dot)",
                ]},
                { "Egg", [
                    "StillFrame (Egg)",
                    "StillFrame (Egg, Repeat)",
                ]},
                { "Falling", [
                    "StillFrame (Falling)",
                    "StillFrame (Falling, Repeat)",
                ]},
                { " Field", [
                    "StillFrame (Field)",
                ]},
                { "Fierce Pose", [
                    "StillFrame (Fierce Pose)",
                    "StillFrame (Fierce Pose, Repeat)",
                ]},
                { "Freezing", [
                    "StillFrame (Freezing 1)",
                    "StillFrame (Freezing 2)",
                    "StillFrame (Freezing 3)",
                ]},
                { "Frozen", [
                    "StillFrame (Frozen)",
                    "StillFrame (Frozen, Repeat)",
                ]},
                { "Full", [
                    "StillFrame (Full)",
                    "StillFrame (Full, Repeat)",
                ]},
                { "Half", [
                    "StillFrame (Half)",
                    "StillFrame (Half, Repeat)",
                ]},
                { "Holding Out Flowers", [
                    "StillFrame (Holding Out Flowers)",
                ]},
                { "Ice Cracking", [
                    "StillFrame (Ice Cracking 1)",
                    "StillFrame (Ice Cracking 2)",
                    "StillFrame (Ice Cracking 3)",
                ]},
                { " Idle", [
                    "StillFrame (Idle)",
                    "StillFrame (Idle, Change to 1)",
                    "StillFrame (Idle, Repeat)",
                ]},
                { "Jumping Up", [
                    "StillFrame (Jumping Up)",
                ]},
                { "Key", [
                    "StillFrame (Key)",
                ]},
                { " Idle (Kneeling)", [
                    "StillFrame (Kneeling 1)",
                    "StillFrame (Kneeling 2)",
                ]},
                { " Idle (KnockedOut)", [
                    "StillFrame (KnockedOut)",
                    "StillFrame (KnockedOut, Repeat)",
                ]},
                { "Light", [
                    "StillFrame (Light)",
                ]},
                { "Lightning", [
                    "StillFrame (Lightning)",
                ]},
                { " LookingUp", [
                    "StillFrame (Looking Up)",
                ]},
                { "MaskOff", [
                    "StillFrame (Mask Off)",
                    "StillFrame (Mask Off, Repeat 1)",
                    "StillFrame (Mask Off, Repeat 2)",
                ]},
                { "Mask", [
                    "StillFrame (Mask)",
                ]},
                { " OnGround", [
                    "StillFrame (OnGround, 1 Frame)",
                ]},
                { "Open", [
                    "StillFrame (Open)",
                    "StillFrame (Open, Repeat)",
                ]},
                { " Pointing", [
                    "StillFrame (Pointing)",
                    "StillFrame (Pointing, Repeat)",
                ]},
                { " Idle (Relaxing)", [
                    "StillFrame (Relaxing)",
                    "StillFrame (Relaxing, Repeat 1)",
                    "StillFrame (Relaxing, Repeat 2)",
                ]},
                { "Rock", [
                    "StillFrame (Rock)",
                    "StillFrame (Rock, Repeat)",
                ]},
                { " Idle (Sitting)", [
                    "StillFrame (Sitting)",
                ]},
                { "Sparkle", [
                    "StillFrame (Sparkle)",
                ]},
                { "Transparent", [
                    "StillFrame (Transparent)",
                ]},
                { "Unfrozen", [
                    "StillFrame (Unfrozen)",
                ]},
                { " Walking", [
                    "StillFrame (Walking 1)",
                    "StillFrame (Walking, Should be 1 Direction)",
                    "Walking (Has Broken/Missing Frames)",
                ]},
            };

            var animNameToCategoryMap = animFrameNamingPriorityByCategory
                .SelectMany(x => x.Value.Select(y => (Category: x.Key, Animation: y)))
                .ToDictionary(x => x.Animation, x => x.Category.Trim());

            var animFrameNamingPriority = animFrameNamingPriorityByCategory
                .SelectMany(x => x.Value)
                .ToArray();

            var animFrameNamingPriorityOrder = animFrameNamingPriority
                .Select((x, i) => (Index: i, Name: x))
                .ToDictionary(x => x.Name, x => x.Index);

            var animNamesNotCoverred = s_animationsByHash.Values
                .Select(x => x.AnimInfo.AnimationName)
                .Distinct()
                .Where(x => !animFrameNamingPriority.Contains(x))
                .OrderBy(x => x)
                .ToArray();

            var animsByFrameNamingPriorityBySpriteVariant = s_animationsByHash.Values
                .GroupBy(x => x.AnimInfo.SpriteName + $" ({x.AnimInfo.Width}x{x.AnimInfo.Height})")
                .ToDictionary(
                    x => x.Key,
                    x => x
                        .Where(y => animFrameNamingPriority.Contains(y.AnimInfo.AnimationName))
                        .OrderByDescending(y => CHR_Utils.DirectionsToFrameCount(y.AnimInfo.Directions))
                        .ThenBy(y => animFrameNamingPriorityOrder[y.AnimInfo.AnimationName])
                        .ToArray()
                );

            foreach (var frame in s_framesByHash.Values)
                if (frame.FrameInfo.FrameName.StartsWith('_'))
                    frame.FrameInfo.FrameName = frame.FrameInfo.FrameName.Substring(1);

            foreach (var spriteVariant in animsByFrameNamingPriorityBySpriteVariant) {
                var categoryIndicies = new Dictionary<string, int>();
                foreach (var anim in spriteVariant.Value) {
                    var categoryName = animNameToCategoryMap[anim.AnimInfo.AnimationName];
                    if (!categoryIndicies.ContainsKey(categoryName))
                        categoryIndicies[categoryName] = 1;

                    foreach (var aniFrame in anim.AnimInfo.AnimationFrames) {
                        if (aniFrame.FrameHashes != null) {
                            var index = 0;
                            var hashCount = aniFrame.FrameHashes.Length;
                            if (anim.AnimInfo.AnimationName.ToLower().Contains("should be 4 directions"))
                                hashCount = 4;
                            else if (anim.AnimInfo.AnimationName.ToLower().Contains("should be 1 direction"))
                                hashCount = 1;

                            var frameHashes = aniFrame.FrameHashes.Take(hashCount).ToArray();
                            var frames = frameHashes
                                .Select(x => (x != null) ? (s_framesByHash.TryGetValue(x ?? "", out var frameOut) ? frameOut : null) : null)
                                .ToArray();

                            // (Edmund has intentional duplicate frames / hash collisions, so don't try and look for frame groups with
                            //  previously-used hashes to add frames to; it will just screw things up.)
                            var firstNamedFrame = (!spriteVariant.Key.StartsWith("Edmund")) ? frames.FirstOrDefault(x => x != null && x.FrameInfo.FrameName.StartsWith('_'))?.FrameInfo : null;
                            foreach (var frame in frames) {
                                if (frame == null || frame.FrameInfo.FrameName.StartsWith('_'))
                                    continue;

                                // If this frame is a new direction for a frame that's already been discovered, use the same name.
                                // This happens with Benard (P1)'s first idle frame, which gets N/S directions from the walking animation.
                                // The Hell Succubus also has this issue with one frame.
                                if (firstNamedFrame != null && !s_framesByHash.Values.Any(x => {
                                    return
                                        x.FrameInfo.FrameName  == firstNamedFrame.FrameName &&
                                        x.FrameInfo.SpriteName == frame.FrameInfo.SpriteName &&
                                        x.FrameInfo.Width      == frame.FrameInfo.Width &&
                                        x.FrameInfo.Height     == frame.FrameInfo.Height &&
                                        x.FrameInfo.Direction  == frame.FrameInfo.Direction;
                                }))
                                    frame.FrameInfo.FrameName = firstNamedFrame.FrameName;
                                else {
                                    if (index == 0) {
                                        index = categoryIndicies[categoryName];
                                        categoryIndicies[categoryName]++;
                                    }
                                    frame.FrameInfo.FrameName = $"_{categoryName} {index}";
                                }
                            }
                        }
                    }
                }
            }

            var animationsWithUnlabeledFrames = s_animationsByHash.Values
                .SelectMany(x => x.AnimInfo.AnimationFrames.Select(y => (Anim: x, Frames: y)).ToArray())
                .Where(x => x.Frames.FrameHashes != null)
                .Where(x => x.Frames.FrameHashes.Any(y => {
                    return y != null && !s_framesByHash[y].FrameInfo.FrameName.StartsWith('_');
                }))
                .Select(x => x.Anim.AnimInfo.AnimationName.PadRight(40) + " | " + x.Anim.ToString())
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            var labeledFrames = 0;
            foreach (var frame in s_framesByHash.Values) {
                if (frame.FrameInfo.FrameName.StartsWith('_')) {
                    frame.FrameInfo.FrameName = frame.FrameInfo.FrameName.Substring(1);
                    labeledFrames++;
                }
            }

            var framesLabeledPercent = labeledFrames * 100.0f / s_framesByHash.Count;
            Console.WriteLine($"Labeled {labeledFrames}/{s_framesByHash.Count} frames ({framesLabeledPercent}%)");

            Console.WriteLine("Animations unaccounted for:");
            foreach (var anim in animationsWithUnlabeledFrames)
                Console.WriteLine($"    {anim}");

            var spriteDefs = CHR_Utils.CreateAllSpriteDefs();

            Console.WriteLine();
            Console.WriteLine("===================================================");
            Console.WriteLine("| WRITING EDITOR RESOURCES                        |");
            Console.WriteLine("===================================================");
            Console.WriteLine();

            Console.WriteLine("Writing new 'SpriteFramesByHash.xml'...");
            using (var file = File.Open(Path.Combine(c_pathOut, "SpriteFramesByHash.xml"), FileMode.Create))
                using (var stream = new StreamWriter(file))
                    CHR_Utils.WriteUniqueFramesByHashXML(stream);

            Console.WriteLine("Writing new 'SpriteAnimationsByHash.xml'...");
            using (var file = File.Open(Path.Combine(c_pathOut, "SpriteAnimationsByHash.xml"), FileMode.Create))
                using (var stream = new StreamWriter(file))
                    CHR_Utils.WriteUniqueAnimationsByHashXML(stream);

            Console.WriteLine();
            Console.WriteLine("===================================================");
            Console.WriteLine("| CREATING SPRITESHEETS                           |");
            Console.WriteLine("===================================================");
            Console.WriteLine();

            var spritesWithSpriteSheets = new HashSet<SpriteDef>();
            foreach (var spriteDef in spriteDefs) {
                var spritePath = Path.Combine(c_pathOut, FilesystemString(spriteDef.Name));
                var spriteSheetVariants = spriteDef.Spritesheets.Values
                    .SelectMany(x => x.Variants)
                    .Select(x => x.Value)
                    .GroupBy(x => (x.Width << 16) + x.Height)
                    .Select(x => x.First())
                    .ToArray();

                List<StandaloneFrameDef> framesFound = [];
                foreach (var variantDef in spriteSheetVariants) {
                    var spriteName = $"{spriteDef.Name} ({variantDef.Width}x{variantDef.Height})";

                    var frames = spriteDef.Spritesheets
                        .SelectMany(x => {
                            var dimensions = SpritesheetDef.KeyToDimensions(x.Key);
                            return x.Value.FrameGroups
                                .SelectMany(y => y.Value.Frames
                                    .Select(z => new StandaloneFrameDef(z.Value, Enum.Parse<SpriteFrameDirection>(z.Key), y.Key, dimensions.Width, dimensions.Height)));
                        })
                        .Where(x => x.Width == variantDef.Width && x.Height == variantDef.Height && s_framesByHash.ContainsKey(x.Hash))
                        .Select(x => new { SpriteDefFrame = x, s_framesByHash[x.Hash].Texture, s_framesByHash[x.Hash].FrameInfo })
                        .OrderBy(x => x.FrameInfo.Width)
                        .ThenBy(x => x.FrameInfo.Height)
                        .ThenBy(x => x.FrameInfo.FrameName)
                        .ThenBy(x => x.FrameInfo.Direction)
                        .ThenBy(x => x.FrameInfo.TextureHash)
                        .ToArray();

                    if (frames.Length == 0)
                        continue;

                    framesFound.AddRange(frames.Select(x => x.SpriteDefFrame));

                    var frameDirections = frames.Select(x => x.FrameInfo.Direction)
                        .Distinct()
                        .ToHashSet();

                    var frameDirectionToIndex = frameDirections
                        .OrderBy(x => x)
                        .Select((x, i) => new { Direction = x, Index = i })
                        .OrderBy(x => x.Direction)
                        .ToDictionary(x => x.Direction, x => x.Index);

                    var filename = FilesystemString(spriteName) + ".BMP";
                    var outputPath = Path.Combine(spritePath, filename);

                    var frameGroups = frames
                        .GroupBy(x => x.FrameInfo.FrameName)
                        .ToDictionary(x => x.Key, x => x.OrderBy(y => y.FrameInfo.Direction).ThenBy(y => y.FrameInfo.TextureHash).ToArray());

                    var frameWidthInPixels  = frames[0].Texture.Width;
                    var frameHeightInPixels = frames[0].Texture.Height;

                    var pixelsPerFrame = frameWidthInPixels * frameHeightInPixels;
                    var frameCount     = frames.Length;
                    var totalPixels    = pixelsPerFrame * frameCount;

                    var imageWidthInFrames  = Math.Max(frameDirections.Count, frameGroups.Max(x => x.Value.Length));
                    var imageHeightInFrames = frameGroups.Count;

                    var imageWidthInPixels  = imageWidthInFrames * frameWidthInPixels;
                    var imageHeightInPixels = imageHeightInFrames * frameHeightInPixels;

                    var newData = new byte[imageWidthInPixels * imageHeightInPixels * 2];

                    int y = 0;
                    bool hasErrors = false;
                    foreach (var frameGroup in frameGroups) {
                        // Normally, frames' X positions are set according to their direction.
                        // But if a frame group had multiple directions -- which, unfortunately, can happen --
                        // set the frames' X position to their index.
                        bool hasDuplicateDirections = frameGroup.Value
                            .GroupBy(x => x.FrameInfo.Direction)
                            .Any(x => x.Count() != 1);

                        // Determine if this is a valid set of frames.
                        var frameGroupDirs = frameGroup.Value.Select(x => x.FrameInfo.Direction).Distinct().ToHashSet();

                        var hasFirst  = frameGroupDirs.Contains(SpriteFrameDirection.First);
                        var hasSecond = frameGroupDirs.Contains(SpriteFrameDirection.Second);

                        var hasS   = frameGroupDirs.Contains(SpriteFrameDirection.S);
                        var hasSSE = frameGroupDirs.Contains(SpriteFrameDirection.SSE);
                        var hasSE  = frameGroupDirs.Contains(SpriteFrameDirection.SE);
                        var hasESE = frameGroupDirs.Contains(SpriteFrameDirection.ESE);
                        var hasE   = frameGroupDirs.Contains(SpriteFrameDirection.E);
                        var hasENE = frameGroupDirs.Contains(SpriteFrameDirection.ENE);
                        var hasNE  = frameGroupDirs.Contains(SpriteFrameDirection.NE);
                        var hasNNE = frameGroupDirs.Contains(SpriteFrameDirection.NNE);
                        var hasN   = frameGroupDirs.Contains(SpriteFrameDirection.N);
                        var hasNNW = frameGroupDirs.Contains(SpriteFrameDirection.NNW);
                        var hasWNW = frameGroupDirs.Contains(SpriteFrameDirection.WNW);
                        var hasWSW = frameGroupDirs.Contains(SpriteFrameDirection.WSW);
                        var hasSSW = frameGroupDirs.Contains(SpriteFrameDirection.SSW);

                        var has1Dir  = frameGroupDirs.Count == 1 && hasFirst;
                        var has2Dirs = frameGroupDirs.Count == 2 && hasFirst && hasSecond;
                        var has4Dirs = frameGroupDirs.Count == 4         && hasSSE && hasESE && hasENE && hasNNE;
                        var has5Dirs = frameGroupDirs.Count == 5 && hasS      && hasSE  && hasE   && hasNE       && hasN;
                        var has6Dirs = frameGroupDirs.Count == 6 && hasS && hasSSE && hasESE && hasENE && hasNNE && hasN;
                        var has8Dirs = frameGroupDirs.Count == 8         && hasSSE && hasESE && hasENE && hasNNE         && hasNNW && hasWNW && hasWSW && hasSSW;
                        var has9Dirs = frameGroupDirs.Count == 9 && hasS && hasSSE && hasSE && hasESE && hasE && hasENE && hasNE && hasNNE && hasN;

                        var hasBogusDirs = !(has1Dir || has2Dirs || has4Dirs || has5Dirs || has6Dirs || has8Dirs || has9Dirs);
                        hasErrors |= hasDuplicateDirections;

                        int frameIndex = 0;
                        foreach (var frame in frameGroup.Value) {
                            int x = ((hasDuplicateDirections) ? frameIndex : frameDirectionToIndex[frame.FrameInfo.Direction]) * frameWidthInPixels;
                            frame.SpriteDefFrame.SpriteSheetX = x;
                            frame.SpriteDefFrame.SpriteSheetY = y;

                            int pos = (y * imageWidthInPixels + x) * 2;
                            var frameData = frame.Texture.BitmapDataARGB1555;

                            int frameDataPos = 0;
                            for (int iy = 0; iy < frameHeightInPixels; iy++) {
                                int ipos = pos + (iy * imageWidthInPixels) * 2;

                                // If there are errors with this frame group, colorize them with a background color.
                                if (false /* hasDuplicateDirections || hasBogusDirs */) {
                                    for (int ix = 0; ix < frameWidthInPixels; ix++) {
                                        var argb1555 = (ushort) ((frameData[frameDataPos + 1] << 8) + frameData[frameDataPos]);
                                        if (argb1555 < 0x8000u) {
                                            newData[ipos++] = (byte) (hasBogusDirs ? 0x1F : 0x00);
                                            newData[ipos++] = (byte) (0x80 | (hasDuplicateDirections ? 0x7C : 0x00));
                                            frameDataPos += 2;
                                        }
                                        else {
                                            newData[ipos++] = frameData[frameDataPos++];
                                            newData[ipos++] = frameData[frameDataPos++];
                                        }
                                    }
                                }
                                else {
                                    for (int ix = 0; ix < frameWidthInPixels * 2; ix++)
                                        newData[ipos++] = frameData[frameDataPos++];
                                }
                            }

                            frameIndex++;
                        }
                        y += frameHeightInPixels;
                    }

#pragma warning disable CA1416 // Validate platform compatibility
                    Console.WriteLine($"Writing '{outputPath}'...");
                    using (var bitmap = new Bitmap(imageWidthInPixels, imageHeightInPixels, PixelFormat.Format16bppArgb1555)) {
                        var bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                        Marshal.Copy(newData, 0, bmpData.Scan0, newData.Length);
                        bitmap.UnlockBits(bmpData);
                        try {
                            _ = Directory.CreateDirectory(Path.Combine(c_pathOut, FilesystemString(spriteDef.Name)));
                            bitmap.Save(outputPath);
                            spritesWithSpriteSheets.Add(spriteDef);
                        }
                        catch { }
                    }
                }
#pragma warning restore CA1416 // Validate platform compatibility

                // Filter out frames that don't have image data that made it to a sprite sheet.
                framesFound = framesFound
                    .Distinct()
                    .OrderBy(x => x.Name)
                    .ThenBy(x => x.Direction)
                    .ThenBy(x => x.Hash)
                    .ToList();

                // Filter out sprite sheets and variants that have no frames.
                spriteDef.Spritesheets = framesFound
                    .OrderBy(x => x.Width)
                    .ThenBy(x => x.Height)
                    .OrderBy(x => x.Name)
                    .ThenBy(x => x.Direction)
                    .GroupBy(x => SpritesheetDef.DimensionsToKey(x.Width, x.Height))
                    .ToDictionary(x => x.Key, x => new SpritesheetDef(
                        x.ToArray(),
                        spriteDef.Spritesheets[x.Key].Variants
                            .Where(y => framesFound.Any(z => y.Value.Width == z.Width && y.Value.Height == z.Height))
                            .OrderBy(y => y.Value.Name)
                            .ToDictionary(y => y.Key, y => y.Value)
                    ));

                var framesFoundHashSet = framesFound.Select(x => x.Hash).ToHashSet();

                // Filter out animations that have missing frames or no frames at all.
                foreach (var spritesheet in spriteDef.Spritesheets.Values) {
                    foreach (var variant in spritesheet.Variants) {
                        var validAnimations = variant.Value.Animations
                            .Where(x => x.AnimationFrames.Length > 0 && x.AnimationFrames
                                .All(y => y.FrameHashes == null || y.FrameHashes.All(z => z == null || framesFoundHashSet.Contains(z)))
                            )
                            .OrderBy(x => x.Name)
                            .ToArray();
                        variant.Value.Animations = validAnimations;
                    }
                }
            }
            Console.WriteLine("Spritesheet writing complete.");

            Console.WriteLine();
            Console.WriteLine("===================================================");
            Console.WriteLine("| CREATING SPRITE DEFS                            |");
            Console.WriteLine("===================================================");
            Console.WriteLine();

            foreach (var spriteDef in spriteDefs) {
                if (!spritesWithSpriteSheets.Contains(spriteDef))
                    continue;

                var spritePath = Path.Combine(c_pathOut, FilesystemString(spriteDef.Name));
                var spriteDefPath = Path.Combine(spritePath, FilesystemString(spriteDef.Name) + ".json");
                Console.WriteLine($"Writing '{spriteDefPath}'...");

                if (spriteDef.Spritesheets.Count == 0) {
                    Console.WriteLine($"    Skipping -- contains no sprite sheets");
                    continue;
                }

                using (var file = File.Open(spriteDefPath, FileMode.Create)) {
                    using (var stream = new StreamWriter(file)) {
                        stream.NewLine = "\n";
                        stream.Write(JsonConvert.SerializeObject(spriteDef, Formatting.Indented));
                    }
                }
            }
            Console.WriteLine("Sprite def writing complete.");
        }

        private static string GetFileString(ScenarioType inputScenario, string filename, ScenarioTableFile chrChpFile) {
            return inputScenario.ToString().PadLeft(11) + ": " + Path.GetFileName(filename).PadLeft(12);
        }

        private static string FilesystemString(string str) {
            return str
                .Replace(" | ", ", ")
                .Replace("|", ",")
                .Replace("?", "X")
                .Replace("-", "_")
                .Replace(":", "_")
                .Replace("/", "_");
        }
    }
}
