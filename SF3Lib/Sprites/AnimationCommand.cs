using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SF3.Types;
using SF3.Utils;

namespace SF3.Sprites {
    public class AnimationCommand : IJsonResource {
        public AnimationCommand() { }

        public AnimationCommand(string[] frameHashes, int duration) {
            FrameGroup        = null;
            FramesByDirection = null;
            FrameHashes       = frameHashes ?? new string[0];
            Command           = SpriteAnimationCommandType.Frame;
            Parameter         = duration;
        }

        public AnimationCommand(SpriteAnimationCommandType command, int parameter) {
            FrameGroup  = null;
            FrameHashes = (command == SpriteAnimationCommandType.Frame) ? new string[0] : null;
            Command     = command;
            Parameter   = parameter;
        }

        /// <summary>
        /// Deserializes a JSON object of a AnimationCommand.
        /// </summary>
        /// <param name="json">AnimationCommand in JSON format as a string.</param>
        /// <returns>A new AnimationCommand if deserializing was successful, or 'null' if not.</returns>
        public static AnimationCommand FromJSON(string json) {
            var aniCommand = new AnimationCommand();
            return aniCommand.AssignFromJSON_String(json) ? aniCommand : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a AnimationCommand.
        /// </summary>
        /// <param name="jToken">AnimationCommand as a JToken.</param>
        /// <returns>A new AnimationCommand if deserializing was successful, or 'null' if not.</returns>
        public static AnimationCommand FromJToken(JToken jToken) {
            var aniCommand = new AnimationCommand();
            return aniCommand.AssignFromJToken(jToken) ? aniCommand : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null)
                return false;

            switch (jToken.Type) {
                case JTokenType.Object:
                    try {
                        var jObj = (JObject) jToken;

                        if (jObj.TryGetValue("Frame", out var frames)) {
                            switch (frames.Type) {
                                case JTokenType.String:
                                    FrameGroup = (string) frames;
                                    break;

                                case JTokenType.Object:
                                    FramesByDirection = ((IDictionary<string, JToken>) frames)
                                        .ToDictionary(x => (SpriteFrameDirection) Enum.Parse(typeof(SpriteFrameDirection), x.Key), x => AnimationCommandFrame.FromJToken(x.Value));
                                    break;
                            }

                            Command   = SpriteAnimationCommandType.Frame;
                            Parameter = jObj.TryGetValue("Duration", out var parameter) ? ((int) parameter) : 0;
                        }
                        else {
                            Command   = jObj.TryGetValue("Command",   out var command)   ? (command.ToObject<SpriteAnimationCommandType>()) : SpriteAnimationCommandType.Frame;
                            Parameter = jObj.TryGetValue("Parameter", out var parameter) ? ((int) parameter) : 0;
                        }
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

        public JToken ToJToken() {
            var jObj = new JObject();

            if (FrameGroup != null || FramesByDirection != null) {
                if (FrameGroup != null)
                    jObj.Add("Frame", new JValue(FrameGroup));
                else if (FramesByDirection != null)
                    jObj.Add("Frame", JToken.FromObject(FramesByDirection.ToDictionary(x => x.Key.ToString(), x => x.Value.ToJToken())));
                jObj.Add("Duration", new JValue(Parameter));
            }
            else {
                jObj.Add("Command", new JValue(Command.ToString()));
                if (Command.NeedsParameter() || Parameter != 0)
                    jObj.Add("Parameter", new JValue(Parameter));
            }

            return jObj;
        }

        public override string ToString() {
            return
                (FrameGroup        != null) ? $"{FrameGroup}, {Parameter}" :
                (FramesByDirection != null) ? string.Join("_", FramesByDirection.Select(x => $"{x.Value} ({x.Key})")) + $", {Parameter}" :
                (FrameHashes       != null) ? string.Join("_", FrameHashes) + $", {Parameter}" :
                $"{Command}_{Parameter}";
        }

        [JsonIgnore]
        public bool HasFrame => Command == SpriteAnimationCommandType.Frame && (FrameGroup != null || FrameHashes != null || FramesByDirection != null);

        public bool HasFullFrame(int directions) {
            return (Command == SpriteAnimationCommandType.Frame) && (
                FrameGroup != null ||
                (FrameHashes != null && FrameHashes.Length == directions && FrameHashes.All(x => x != null)) ||
                (FramesByDirection != null && FramesByDirection.Count == directions && Enumerable
                    .Range(0, directions)
                    .Select(x => CHR_Utils.FrameNumberToSpriteDir(directions, x))
                    .All(x => FramesByDirection.ContainsKey(x))
                )
            );
        }

        public string FrameGroup;
        public Dictionary<SpriteFrameDirection, AnimationCommandFrame> FramesByDirection;
        public string[] FrameHashes;

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteAnimationCommandType Command;

        public int Parameter;
    };
}
