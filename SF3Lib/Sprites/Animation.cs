using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.Types;
using SF3.Utils;

namespace SF3.Sprites {
    public class Animation : IJsonResource {
        public Animation() { }

        public Animation(UniqueAnimationDef aniInfo) : this() {
            AnimationCommands = aniInfo.AnimationCommands;
        }

        /// <summary>
        /// Deserializes a JSON object of a Animation.
        /// </summary>
        /// <param name="json">Animation in JSON format as a string.</param>
        /// <returns>A new Animation if deserializing was successful, or 'null' if not.</returns>
        public static Animation FromJSON(string json) {
            var animation = new Animation();
            return animation.AssignFromJSON_String(json) ? animation : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a Animation.
        /// </summary>
        /// <param name="jToken">Animation as a JToken.</param>
        /// <returns>A new Animation if deserializing was successful, or 'null' if not.</returns>
        public static Animation FromJToken(JToken jToken) {
            var animation = new Animation();
            return animation.AssignFromJToken(jToken) ? animation : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null)
                return false;

            switch (jToken.Type) {
                case JTokenType.Array:
                    try {
                        AnimationCommands = jToken.Select(x => AnimationCommand.FromJToken(x)).ToArray();
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
        public bool HasAllFrames(SpriteDirectionCountType startDirections) {
            var currentDirections = startDirections;
            foreach (var aniCommand in AnimationCommands) {
                if (aniCommand.Command == SpriteAnimationCommandType.SetDirectionCount)
                    currentDirections = (SpriteDirectionCountType) aniCommand.Parameter;
                else if (aniCommand.Command == SpriteAnimationCommandType.Frame && !aniCommand.HasFullFrame(CHR_Utils.DirectionsToFrameCount(currentDirections)))
                    return false;
            }
            return true;
        }

        public override string ToString() => string.Join(", ", AnimationCommands.Select(x => x.ToString()));

        public AnimationCommand[] AnimationCommands;
    };
}
