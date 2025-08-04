using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.Types;
using SF3.Utils;

namespace SF3.Sprites {
    public class FrameGroup {
        public FrameGroup() { }

        public FrameGroup(UniqueFrameDef[] frames) {
            Frames = frames.ToDictionary(x => x.Direction, x => new Frame(x));
        }

        public FrameGroup(StandaloneFrameDef[] frames) {
            Frames = frames.ToDictionary(x => x.Direction, x => new Frame(x));
        }

        /// <summary>
        /// Deserializes a JSON object of a FrameGroup.
        /// </summary>
        /// <param name="json">FrameGroup in JSON format as a string.</param>
        /// <returns>A new FrameGroup if deserializing was successful, or 'null' if not.</returns>
        public static FrameGroup FromJSON(string json, int frameHeight) {
            var frameGroup = new FrameGroup();
            return frameGroup.AssignFromJSON_String(json, frameHeight) ? frameGroup : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a FrameGroup.
        /// </summary>
        /// <param name="jToken">FrameGroup as a JToken.</param>
        /// <returns>A new FrameGroup if deserializing was successful, or 'null' if not.</returns>
        public static FrameGroup FromJToken(JToken jToken, int frameHeight) {
            var frameGroup = new FrameGroup();
            return frameGroup.AssignFromJToken(jToken, frameHeight) ? frameGroup : null;
        }

        public bool AssignFromJSON_String(string json, int frameHeight)
            => AssignFromJToken(JToken.Parse(json), frameHeight);

        public bool AssignFromJToken(JToken jToken, int frameHeight) {
            if (jToken == null)
                return false;

            switch (jToken.Type) {
                case JTokenType.Object:
                    try {
                        var jObj = (JObject) jToken;
                        if (jObj.ContainsKey("Directions")) {
                            var directions   = (int) jObj["Directions"];
                            var spritesheetX = (int) jObj["SpritesheetX"];
                            var spritesheetY = (int) jObj["SpritesheetY"];
                            var hashes       = jObj["Hashes"].Select(x => (string) x).ToArray();

                            var frameDirs = SpriteUtils.SpritesheetFrameGroupDirections(directions);
                            Frames = frameDirs
                                .Select((x, i) => (Direction: x, Index: i))
                                .ToDictionary(x => x.Direction, x => new Frame() {
                                    SpritesheetX = spritesheetX,
                                    SpritesheetY = spritesheetY + (x.Index * frameHeight),
                                    Hash = hashes[x.Index]
                                });
                        }
                        else {
                            Frames = ((IDictionary<string, JToken>) jToken)
                                .ToDictionary(x => (SpriteFrameDirection) Enum.Parse(typeof(SpriteFrameDirection), x.Key), x => Frame.FromJToken(x.Value));
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

        public string ToJSON_String(int frameHeight)
            => ToJToken(frameHeight).ToString(Formatting.Indented);

        public JToken ToJToken(int frameHeight) {
            if (Frames == null)
                return null;

            // If the frames are standard and stacked in a very specific way, we can simplify the declaration.
            // This is a big improvement for most sprites.
            var directions = SpriteUtils.SpritesheetFrameGroupDirections(Frames.Count);
            bool FramesAreStacked(out int spritesheetXOut, out int spritesheetYOut) {
                spritesheetXOut = -1;
                spritesheetYOut = -1;

                if (Frames.Count == 0)
                    return false;

                // Ensure that the frame directions are standard.
                if (directions == null)
                    return false;
                if (!directions.All(x => Frames.ContainsKey(x)))
                    return false;

                // Ensure that the frames are stacked upon another vertically.
                int? frameX = null;
                int? frameY = null;
                int? topFrameY = null;
                foreach (var dir in directions) {
                    var frame = Frames[dir];
                    if (!topFrameY.HasValue) {
                        frameX = frame.SpritesheetX;
                        frameY = frame.SpritesheetY;
                        topFrameY = frameY;
                    }
                    else {
                        frameY += frameHeight;
                        if (frame.SpritesheetX != frameX || frame.SpritesheetY != frameY)
                            return false;
                    }
                }

                // Checks passed; this frame group can be simplified.
                spritesheetXOut = frameX.Value;
                spritesheetYOut = topFrameY.Value;
                return true;
            }

            if (FramesAreStacked(out var spritesheetX, out var spritesheetY)) {
                return new JObject {
                    { "Directions", new JValue(Frames.Count) },
                    { "SpritesheetX", new JValue(spritesheetX) },
                    { "SpritesheetY", new JValue(spritesheetY) },
                    { "Hashes", JToken.FromObject(directions.Select(x => Frames[x].Hash)) }
                };
            }
            else
                return JToken.FromObject(Frames.ToDictionary(x => x.Key.ToString(), x => x.Value.ToJToken()));
        }

        public override string ToString() => string.Join(", ", Frames.Keys);

        public Dictionary<SpriteFrameDirection, Frame> Frames;
    }
}
