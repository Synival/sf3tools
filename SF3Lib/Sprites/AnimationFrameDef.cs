using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SF3.Types;
using SF3.Utils;

namespace SF3.Sprites {
    public class AnimationFrameDef {
        public AnimationFrameDef() { }

        public AnimationFrameDef(string[] frameHashes, int duration) {
            FrameGroup  = null;
            Frames      = null;
            FrameHashes = frameHashes ?? new string[0];
            Command     = SpriteAnimationFrameCommandType.Frame;
            Parameter   = duration;
        }

        public AnimationFrameDef(SpriteAnimationFrameCommandType command, int parameter) {
            FrameGroup  = null;
            FrameHashes = (command == SpriteAnimationFrameCommandType.Frame) ? new string[0] : null;
            Command     = command;
            Parameter   = parameter;
        }

        /// <summary>
        /// Deserializes a JSON object of a AnimationFrameDef.
        /// </summary>
        /// <param name="json">AnimationFrameDef in JSON format as a string.</param>
        /// <returns>A new AnimationFrameDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationFrameDef FromJSON(string json)
            => FromJToken(JToken.Parse(json));

        /// <summary>
        /// Deserializes a JSON object of a AnimationFrameDef.
        /// </summary>
        /// <param name="jToken">AnimationFrameDef as a JToken.</param>
        /// <returns>A new AnimationFrameDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationFrameDef FromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return null;

            try {
                var jObj = (JObject) jToken;
                var newDef = new AnimationFrameDef();

                newDef.FrameGroup = jObj.TryGetValue("FrameGroup", out var frameGroup) ? ((string) frameGroup) : null;

                if (jObj.TryGetValue("Frames", out var frames) && frames.Type == JTokenType.Object) {
                    newDef.Frames = ((IDictionary<string, JToken>) frames)
                        .ToDictionary(x => x.Key, x => AnimationFrameDirectionDef.FromJToken(x.Value));
                }

                newDef.FrameHashes = jObj.TryGetValue("FrameHashes", out var frameHashes)
                    ? frameHashes.Select(x => (string) x).ToArray()
                    : null;

                newDef.Command   = jObj.TryGetValue("Command",   out var command)   ? (command.ToObject<SpriteAnimationFrameCommandType>()) : SpriteAnimationFrameCommandType.Frame;
                newDef.Parameter = jObj.TryGetValue("Parameter", out var parameter) ? ((int) parameter) : 0;

                return newDef;
            }
            catch {
                return null;
            }
        }

        public bool ConvertFrameHashes(Dictionary<string, FrameGroupDef> frameGroups) {
            if (frameGroups == null || FrameHashes == null)
                return false;

            var frameCount = FrameHashes.Length;
            if (FrameHashes.All(x => x != null)) {
                bool FrameGroupHasHashes(FrameGroupDef fg) {
                    return FrameHashes
                        .Select((x, i) => (Hash: x, Dir: CHR_Utils.FrameNumberToSpriteDir(frameCount, i)))
                        .All(x => fg.Frames.ContainsKey(x.Dir) && fg.Frames[x.Dir].Hash == x.Hash);
                }

                var frameGroup = frameGroups
                    .FirstOrDefault(x => FrameGroupHasHashes(x.Value));
                if (frameGroup.Value != null) {
                    FrameGroup  = frameGroup.Key;
                    Frames      = null;
                    FrameHashes = null;
                    return true;
                }
            }

            var allFramesByHash = frameGroups
                .SelectMany(x => x.Value.Frames.Select(y => (
                    Name: x.Key,
                    Dir: y.Key,
                    Frame: y.Value)))
                .ToDictionary(x => x.Frame.Hash, x => x);

            var frames = FrameHashes
                .Select((x, i) => (Dir: CHR_Utils.FrameNumberToSpriteDir(frameCount, i), Hash: x))
                .Where(x => x.Hash != null)
                .ToDictionary(x => x.Dir.ToString(), x => {
                    var frame = allFramesByHash.TryGetValue(x.Hash, out var frameOut) ? frameOut : default;
                    return (frame.Frame != null) ? new AnimationFrameDirectionDef() { Frame = frame.Name, Direction = frame.Dir } : null;
                });

            var nonNullFrameCount = FrameHashes.Count(x => x != null);
            var foundFrameCount = frames.Count(x => x.Value != null);
            if (foundFrameCount == nonNullFrameCount) {
                FrameGroup  = null;
                Frames      = frames;
                FrameHashes = null;
                return true;
            }

            // We should never reach here!
            throw new InvalidOperationException("Animation has frames that don't exist!");
        }

        public override string ToString() {
            return
                (FrameGroup != null)  ? $"{FrameGroup}, {Parameter}" :
                (Frames != null)      ? string.Join("_", Frames.Select(x => $"{x.Value} ({x.Key})")) + $", {Parameter}" :
                (FrameHashes != null) ? string.Join("_", FrameHashes) + $", {Parameter}" :
                $"{Command}_{Parameter}";
        }

        [JsonIgnore]
        public bool HasFrame => Command == SpriteAnimationFrameCommandType.Frame && (FrameGroup != null || FrameHashes != null || Frames != null);

        public bool HasFullFrame(int directions) {
            return (Command == SpriteAnimationFrameCommandType.Frame) && (
                FrameGroup != null ||
                (FrameHashes != null && FrameHashes.Length == directions && FrameHashes.All(x => x != null)) ||
                (Frames != null && Frames.Count == directions && Enumerable
                    .Range(0, directions)
                    .Select(x => CHR_Utils.FrameNumberToSpriteDir(directions, x).ToString())
                    .All(x => Frames.ContainsKey(x))
                )
            );
        }

        public string FrameGroup;
        public Dictionary<string, AnimationFrameDirectionDef> Frames;
        public string[] FrameHashes;

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteAnimationFrameCommandType Command;

        public int Parameter;
    };
}
