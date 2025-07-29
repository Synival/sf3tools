using System.Collections.Generic;
using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SF3.Sprites {
    public class Spritesheet : IJsonResource {
        public Spritesheet() { }

        public Spritesheet(UniqueFrameDef[] frames, UniqueAnimationDef[] animations) {
            FrameGroupsByName = frames
                .GroupBy(x => x.FrameName)
                .ToDictionary(x => x.Key, x => new FrameGroup(x.ToArray()));

            AnimationSetsByDirections = GetAnimationGroupsByDirections(animations);
        }

        public Spritesheet(StandaloneFrameDef[] frames, UniqueAnimationDef[] animations) {
            FrameGroupsByName = frames
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => new FrameGroup(x.ToArray()));

            AnimationSetsByDirections = GetAnimationGroupsByDirections(animations);
        }

        public Spritesheet(StandaloneFrameDef[] frames, Dictionary<int, AnimationSet> variants) {
            FrameGroupsByName = frames
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => new FrameGroup(x.ToArray()));

            AnimationSetsByDirections = variants;
        }

        /// <summary>
        /// Deserializes a JSON object of a Spritesheet.
        /// </summary>
        /// <param name="json">Spritesheet in JSON format as a string.</param>
        /// <returns>A new Spritesheet if deserializing was successful, or 'null' if not.</returns>
        public static Spritesheet FromJSON(string json) {
            var spritesheet = new Spritesheet();
            return spritesheet.AssignFromJSON_String(json) ? spritesheet : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a Spritesheet.
        /// </summary>
        /// <param name="jToken">Spritesheet as a JToken.</param>
        /// <returns>A new Spritesheet if deserializing was successful, or 'null' if not.</returns>
        public static Spritesheet FromJToken(JToken jToken) {
            var spritesheet = new Spritesheet();
            return spritesheet.AssignFromJToken(jToken) ? spritesheet : null;
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
                            FrameGroupsByName = ((IDictionary<string, JToken>) frameGroups)
                                .ToDictionary(x => x.Key, x => FrameGroup.FromJToken(x.Value));
                        }

                        if (jObj.TryGetValue("AnimationByDirections", out var animationByDirections) && animationByDirections.Type == JTokenType.Object) {
                            AnimationSetsByDirections = ((IDictionary<string, JToken>) animationByDirections)
                                .Where(x => int.TryParse(x.Key, out var _))
                                .ToDictionary(x => int.Parse(x.Key), x => AnimationSet.FromJToken(x.Value));
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

            if (FrameGroupsByName != null)
                jObj.Add("FrameGroups", JToken.FromObject(FrameGroupsByName.ToDictionary(x => x.Key, x => x.Value.ToJToken())));
            if (AnimationSetsByDirections != null)
                jObj.Add("AnimationByDirections", JToken.FromObject(AnimationSetsByDirections.ToDictionary(x => x.Key, x => x.Value.ToJToken())));

            return jObj;
        }

        private Dictionary<int, AnimationSet> GetAnimationGroupsByDirections(UniqueAnimationDef[] animations) {
            return animations
                .Where(y => y.AnimationCommands != null && y.AnimationCommands.Length > 0)
                .OrderBy(y => y.Width)
                .ThenBy(y => y.Height)
                .ThenBy(y => y.Directions)
                .GroupBy(y => ((y.Width & 0xFFFF) << 24) + ((y.Height & 0xFFFF) << 8) + (y.Directions & 0xFF))
                .ToDictionary(y => y.First().Directions, y => new AnimationSet(y.ToArray()));
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
        public Dictionary<int, AnimationSet> AnimationSetsByDirections;
    }
}
