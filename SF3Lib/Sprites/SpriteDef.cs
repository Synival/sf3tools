using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SF3.Sprites {
    public class SpriteDef {
        public SpriteDef() { }

        public SpriteDef(string name, UniqueFrameDef[] frames, UniqueAnimationDef[] animations) {
            var frameKeys = frames.Select(x => SpritesheetDef.DimensionsToKey(x.Width, x.Height)).Distinct().ToArray();
            var animationKeys = animations.Select(x => SpritesheetDef.DimensionsToKey(x.Width, x.Height)).Distinct().ToArray();
            var keys = frameKeys.Concat(animationKeys).Distinct().OrderBy(x => x).ToArray();

            Name = name;
            Spritesheets = keys
                .Select(x => SpritesheetDef.KeyToDimensions(x))
                .OrderBy(x => x.Width)
                .ThenBy(x => x.Height)
                .ToDictionary(x => SpritesheetDef.DimensionsToKey(x.Width, x.Height), x => new SpritesheetDef(
                    frames.Where(y => y.Width == x.Width && y.Height == x.Height).OrderBy(y => y.FrameName).ThenBy(y => y.Direction).ThenBy(y => y.TextureHash).ToArray(),
                    animations.Where(y => y.Width == x.Width && y.Height == x.Height).OrderBy(y => y.Directions).ThenBy(y => y.AnimationName).ThenBy(y => y.AnimationHash).ToArray()
                ));

            foreach (var spritesheet in Spritesheets.Values)
                foreach (var variant in spritesheet.AnimationByDirections)
                    foreach (var animation in variant.Value.Animations.Values)
                        foreach (var aniFrame in animation.AnimationFrames)
                            aniFrame.ConvertFrameHashes(spritesheet.FrameGroups);
        }

        public SpriteDef(string name, StandaloneFrameDef[] frames, UniqueAnimationDef[] animations) {
            var frameKeys = frames.Select(x => SpritesheetDef.DimensionsToKey(x.Width, x.Height)).Distinct().ToArray();
            var animationKeys = animations.Select(x => SpritesheetDef.DimensionsToKey(x.Width, x.Height)).Distinct().ToArray();
            var keys = frameKeys.Concat(animationKeys).Distinct().OrderBy(x => x).ToArray();

            Name = name;
            Spritesheets = keys
                .Select(x => SpritesheetDef.KeyToDimensions(x))
                .OrderBy(x => x.Width)
                .ThenBy(x => x.Height)
                .ToDictionary(x => SpritesheetDef.DimensionsToKey(x.Width, x.Height), x => new SpritesheetDef(
                    frames.Where(y => y.Width == x.Width && y.Height == x.Height).OrderBy(y => y.Name).ThenBy(y => y.Direction).ThenBy(y => y.Hash).ToArray(),
                    animations.Where(y => y.Width == x.Width && y.Height == x.Height).OrderBy(y => y.Directions).ThenBy(y => y.AnimationName).ThenBy(y => y.AnimationHash).ToArray()
                ));
        }

        /// <summary>
        /// Deserializes a JSON object of a SpriteDef.
        /// </summary>
        /// <param name="json">SpriteDef in JSON format as a string.</param>
        /// <returns>A new SpriteDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteDef FromJSON(string json) {
            try {
                return JsonConvert.DeserializeObject<SpriteDef>(json);
            }
            catch {
                return null;
            }
        }

        /// <summary>
        /// Converts the SpriteDef to a JSON object string.
        /// </summary>
        /// <returns>A string in JSON format.</returns>
        public string ToJSON_String() {
            var settings = new JsonSerializerSettings() {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };
            return JsonConvert.SerializeObject(this, settings);
        }

        public override string ToString() => Name;

        public string Name;
        public Dictionary<string, SpritesheetDef> Spritesheets;
    }
}
