using System.Collections.Generic;
using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SF3.Sprites {
    public class SpritesheetDef : IJsonResource {
        public SpritesheetDef() { }

        public SpritesheetDef(UniqueFrameDef[] frames, UniqueAnimationDef[] animations) {
            FrameGroups = frames
                .GroupBy(x => x.FrameName)
                .ToDictionary(x => x.Key, x => new FrameGroupDef(x.ToArray()));

            AnimationByDirections = GetAnimationGroupsByDirections(animations);
        }

        public SpritesheetDef(StandaloneFrameDef[] frames, UniqueAnimationDef[] animations) {
            FrameGroups = frames
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => new FrameGroupDef(x.ToArray()));

            AnimationByDirections = GetAnimationGroupsByDirections(animations);
        }

        public SpritesheetDef(StandaloneFrameDef[] frames, Dictionary<int, AnimationGroupDef> variants) {
            FrameGroups = frames
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => new FrameGroupDef(x.ToArray()));

            AnimationByDirections = variants;
        }

        /// <summary>
        /// Deserializes a JSON object of a SpritesheetDef.
        /// </summary>
        /// <param name="json">SpritesheetDef in JSON format as a string.</param>
        /// <returns>A new SpritesheetDef if deserializing was successful, or 'null' if not.</returns>
        public static SpritesheetDef FromJSON(string json) {
            var spritesheetDef = new SpritesheetDef();
            return spritesheetDef.AssignFromJSON_String(json) ? spritesheetDef : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a SpritesheetDef.
        /// </summary>
        /// <param name="jToken">SpritesheetDef as a JToken.</param>
        /// <returns>A new SpritesheetDef if deserializing was successful, or 'null' if not.</returns>
        public static SpritesheetDef FromJToken(JToken jToken) {
            var spritesheetDef = new SpritesheetDef();
            return spritesheetDef.AssignFromJToken(jToken) ? spritesheetDef : null;
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

                        SpriteID       = jObj.TryGetValue("SpriteID",       out var spriteId)       ? ((int) spriteId)       : 0;
                        VerticalOffset = jObj.TryGetValue("VerticalOffset", out var verticalOffset) ? ((int) verticalOffset) : 0;
                        Unknown0x08    = jObj.TryGetValue("Unknown0x08",    out var unknown0x08)    ? ((int) unknown0x08)    : 0;
                        CollisionSize  = jObj.TryGetValue("CollisionSize",  out var collisionSize)  ? ((int) collisionSize)  : 0;
                        Scale          = jObj.TryGetValue("Scale",          out var scale)          ? ((float) scale)        : 0.0f;

                        if (jObj.TryGetValue("FrameGroups", out var frameGroups) && frameGroups.Type == JTokenType.Object) {
                            FrameGroups = ((IDictionary<string, JToken>) frameGroups)
                                .ToDictionary(x => x.Key, x => FrameGroupDef.FromJToken(x.Value));
                        }

                        if (jObj.TryGetValue("AnimationByDirections", out var animationByDirections) && animationByDirections.Type == JTokenType.Object) {
                            AnimationByDirections = ((IDictionary<string, JToken>) animationByDirections)
                                .Where(x => int.TryParse(x.Key, out var _))
                                .ToDictionary(x => int.Parse(x.Key), x => AnimationGroupDef.FromJToken(x.Value));
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

            jObj.Add("SpriteID",       new JValue(SpriteID));
            jObj.Add("VerticalOffset", new JValue(VerticalOffset));
            jObj.Add("Unknown0x08",    new JValue(Unknown0x08));
            jObj.Add("CollisionSize",  new JValue(CollisionSize));
            jObj.Add("Scale",          new JValue(Scale));

            if (FrameGroups != null)
                jObj.Add("FrameGroups", JToken.FromObject(FrameGroups.ToDictionary(x => x.Key, x => x.Value.ToJToken())));
            if (AnimationByDirections != null)
                jObj.Add("AnimationByDirections", JToken.FromObject(AnimationByDirections.ToDictionary(x => x.Key, x => x.Value.ToJToken())));

            return jObj;
        }

        private Dictionary<int, AnimationGroupDef> GetAnimationGroupsByDirections(UniqueAnimationDef[] animations) {
            return animations
                .Where(y => y.AnimationCommands != null && y.AnimationCommands.Length > 0)
                .OrderBy(y => y.Width)
                .ThenBy(y => y.Height)
                .ThenBy(y => y.Directions)
                .GroupBy(y => ((y.Width & 0xFFFF) << 24) + ((y.Height & 0xFFFF) << 8) + (y.Directions & 0xFF))
                .ToDictionary(y => y.First().Directions, y => new AnimationGroupDef(y.ToArray()));
        }

        public static string DimensionsToKey(int width, int height)
            => $"{width}x{height}";

        public static (int Width, int Height) KeyToDimensions(string key) {
            var xPos = key.IndexOf('x');
            return (Width: int.Parse(key.Substring(0, xPos)), Height: int.Parse(key.Substring(xPos + 1)));
        }

        public override string ToString() => string.Join(", ", FrameGroups.Keys);

        public int SpriteID       = 0;
        public int VerticalOffset = 0;
        public int Unknown0x08    = 0;
        public int CollisionSize  = 0;
        public float Scale        = 0;

        public Dictionary<string, FrameGroupDef> FrameGroups;
        public Dictionary<int, AnimationGroupDef> AnimationByDirections;
    }
}
