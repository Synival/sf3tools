using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.Types;

namespace SF3.Sprites {
    public class Spritesheet {
        public Spritesheet() { }

        /// <summary>
        /// Deserializes a JSON object of a Spritesheet.
        /// </summary>
        /// <param name="json">Spritesheet in JSON format as a string.</param>
        /// <returns>A new Spritesheet if deserializing was successful, or 'null' if not.</returns>
        public static Spritesheet FromJSON(string json, int frameHeight) {
            var spritesheet = new Spritesheet();
            return spritesheet.AssignFromJSON_String(json, frameHeight) ? spritesheet : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a Spritesheet.
        /// </summary>
        /// <param name="jToken">Spritesheet as a JToken.</param>
        /// <returns>A new Spritesheet if deserializing was successful, or 'null' if not.</returns>
        public static Spritesheet FromJToken(JToken jToken, int frameHeight) {
            var spritesheet = new Spritesheet();
            return spritesheet.AssignFromJToken(jToken, frameHeight) ? spritesheet : null;
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

                        SpriteID       = jObj.TryGetValue("SpriteID",       out var spriteId)       ? ((int) spriteId)       : 0;
                        VerticalOffset = jObj.TryGetValue("VerticalOffset", out var verticalOffset) ? ((int) verticalOffset) : 0;
                        Unknown0x08    = jObj.TryGetValue("Unknown0x08",    out var unknown0x08)    ? ((int) unknown0x08)    : 0;
                        CollisionSize  = jObj.TryGetValue("CollisionSize",  out var collisionSize)  ? ((int) collisionSize)  : 0;
                        Scale          = jObj.TryGetValue("Scale",          out var scale)          ? ((float) scale)        : 0.0f;

                        if (jObj.TryGetValue("FrameGroups", out var frameGroups) && frameGroups.Type == JTokenType.Object) {
                            FrameGroupsByName = ((IDictionary<string, JToken>) frameGroups)
                                .ToDictionary(x => x.Key, x => FrameGroup.FromJToken(x.Value, frameHeight));
                        }

                        if (jObj.TryGetValue("AnimationByDirections", out var animationByDirections) && animationByDirections.Type == JTokenType.Object) {
                            AnimationSetsByDirections = ((IDictionary<string, JToken>) animationByDirections)
                                .Where(x => Enum.TryParse<SpriteDirectionCountType>(x.Key, out var _))
                                .ToDictionary(x => SpriteDirectionCountTypeExtensions.FromSerializedString(x.Key), x => AnimationSet.FromJToken(x.Value));
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
            var jObj = new JObject {
                { "SpriteID",       new JValue(SpriteID) },
                { "VerticalOffset", new JValue(VerticalOffset) },
                { "Unknown0x08",    new JValue(Unknown0x08) },
                { "CollisionSize",  new JValue(CollisionSize) },
                { "Scale",          new JValue(Scale) }
            };

            if (FrameGroupsByName != null)
                jObj.Add("FrameGroups", JToken.FromObject(FrameGroupsByName.ToDictionary(x => x.Key, x => x.Value.ToJToken(frameHeight))));
            if (AnimationSetsByDirections != null)
                jObj.Add("AnimationByDirections", JToken.FromObject(AnimationSetsByDirections.ToDictionary(x => x.Key.ToSerializedString(), x => x.Value.ToJToken())));

            return jObj;
        }

        public static string DimensionsToKey(int width, int height)
            => $"{width}x{height}";

        public static (int Width, int Height) KeyToDimensions(string key) {
            var xPos = key.IndexOf('x');
            return (Width: int.Parse(key.Substring(0, xPos)), Height: int.Parse(key.Substring(xPos + 1)));
        }

        public override string ToString() => string.Join(", ", FrameGroupsByName.Keys);

        public int SpriteID       = 0;
        public int VerticalOffset = 0;
        public int Unknown0x08    = 0;
        public int CollisionSize  = 0;
        public float Scale        = 0;

        public Dictionary<string, FrameGroup> FrameGroupsByName;
        public Dictionary<SpriteDirectionCountType, AnimationSet> AnimationSetsByDirections;
    }
}
