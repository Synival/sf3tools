using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.Types;
using SF3.Utils;

namespace SF3.Sprites {
    public class AnimationDef : IJsonResource {
        public AnimationDef() { }

        public AnimationDef(UniqueAnimationDef aniInfo) : this() {
            AnimationCommands = aniInfo.AnimationCommands;
        }

        /// <summary>
        /// Deserializes a JSON object of a AnimationDef.
        /// </summary>
        /// <param name="json">AnimationDef in JSON format as a string.</param>
        /// <returns>A new AnimationDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationDef FromJSON(string json) {
            var animationDef = new AnimationDef();
            return animationDef.AssignFromJSON_String(json) ? animationDef : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a AnimationDef.
        /// </summary>
        /// <param name="jToken">AnimationDef as a JToken.</param>
        /// <returns>A new AnimationDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationDef FromJToken(JToken jToken) {
            var animationDef = new AnimationDef();
            return animationDef.AssignFromJToken(jToken) ? animationDef : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null)
                return false;

            switch (jToken.Type) {
                case JTokenType.Array:
                    try {
                        AnimationCommands = jToken.Select(x => AnimationCommandDef.FromJToken(x)).ToArray();
                    }
                    catch {
                        return false;
                    }
                    return true;

                default:
                    return false;
            }
        }

        public string ToJSON_String()
            => ToJToken().ToString(Formatting.Indented);

        public JToken ToJToken()
            => JToken.FromObject(AnimationCommands.Select(x => x.ToJToken()).ToArray());

        /// <summary>
        /// Returns 'true' if all of the animation commands with frames have no 'null' entries.
        /// </summary>
        /// <param name="startDirections">The number directions this animation has before any 'SetDirectionCount' command.</param>
        /// <returns>'true' if there are no frame groups with missing frames, otherwise 'false'.</returns>
        public bool HasAllFrames(int startDirections) {
            var currentDirections = startDirections;
            foreach (var aniCommand in AnimationCommands) {
                if (aniCommand.Command == SpriteAnimationFrameCommand.SetDirectionCount)
                    currentDirections = aniCommand.Parameter;
                else if (aniCommand.Command == SpriteAnimationFrameCommand.Frame && !aniCommand.HasFullFrame(CHR_Utils.DirectionsToFrameCount(currentDirections)))
                    return false;
            }
            return true;
        }

        public override string ToString() => string.Join(", ", AnimationCommands.Select(x => x.ToString()));

        public AnimationCommandDef[] AnimationCommands;
    };
}
