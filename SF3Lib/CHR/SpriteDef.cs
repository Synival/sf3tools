using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.Sprites;
using SF3.Utils;

namespace SF3.CHR {
    public class SpriteDef : IJsonResource {
        /// <summary>
        /// Deserializes a JSON object of a SpriteDef.
        /// </summary>
        /// <param name="json">SpriteDef in JSON format as a string.</param>
        /// <returns>A new SpriteDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteDef FromJSON(string json) {
            var spriteDef = new SpriteDef();
            return spriteDef.AssignFromJSON_String(json) ? spriteDef : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a SpriteDef.
        /// </summary>
        /// <param name="jToken">SpriteDef as a JToken.</param>
        /// <returns>A new SpriteDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteDef FromJToken(JToken jToken) {
            var spriteDef = new SpriteDef();
            return spriteDef.AssignFromJToken(jToken) ? spriteDef : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return false;

            try {
                var jObj = (JObject) jToken;

                SpriteName     = jObj.TryGetValue("Name",           out var spriteName)     ? ((string) spriteName)  : null;
                Width          = jObj.TryGetValue("Width",          out var width)          ? ((int) width)          : 0;
                Height         = jObj.TryGetValue("Height",         out var height)         ? ((int) height)         : 0;
                Directions     = jObj.TryGetValue("Directions",     out var directions)     ? ((int) directions)     : 0;
                PromotionLevel = jObj.TryGetValue("PromotionLevel", out var promotionLevel) ? ((int) promotionLevel) : 0;

                var spriteDef = SpriteUtils.GetSpriteDef(SpriteName);
                var spritesheet = (spriteDef?.Spritesheets?.TryGetValue(SpritesheetDef.DimensionsToKey(Width, Height), out var ssOut) == true) ? ssOut : null;

                SpriteID       = jObj.TryGetValue("SpriteID",       out var spriteId)       ? ((int) spriteId)       : spritesheet?.SpriteID       ?? 0;
                VerticalOffset = jObj.TryGetValue("VerticalOffset", out var verticalOffset) ? ((int) verticalOffset) : spritesheet?.VerticalOffset ?? 0;
                Unknown0x08    = jObj.TryGetValue("Unknown0x08",    out var unknown0x08)    ? ((int) unknown0x08)    : spritesheet?.Unknown0x08    ?? 0;
                CollisionSize  = jObj.TryGetValue("CollisionSize",  out var collisionSize)  ? ((int) collisionSize)  : spritesheet?.CollisionSize  ?? 0;
                Scale          = jObj.TryGetValue("Scale",          out var scale)          ? ((float) scale)        : spritesheet?.Scale          ?? 0;

                SpriteFrames = jObj.TryGetValue("Frames", out var spriteFrames)
                    ? spriteFrames.Select(x => SpriteFramesDef.FromJToken(x)).ToArray()
                    : null;

                SpriteAnimations = jObj.TryGetValue("Animations", out var spriteAnimations)
                    ? spriteAnimations.Select(x => SpriteAnimationsDef.FromJToken(x)).ToArray()
                    : null;

                return true;
            }
            catch {
                return false;
            }
        }

        public string ToJSON_String()
            => ToJToken().ToString(Formatting.Indented);

        public JToken ToJToken() {
            var jObj = new JObject();
            var jsonSettings = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            jObj.Add("Name",       new JValue(SpriteName));
            jObj.Add("Width",      new JValue(Width));
            jObj.Add("Height",     new JValue(Height));
            jObj.Add("Directions", new JValue(Directions));
            if (PromotionLevel != 0)
                jObj.Add("PromotionLevel", new JValue(PromotionLevel));

            var spriteDef = SpriteUtils.GetSpriteDef(SpriteName);
            var spritesheet = (spriteDef?.Spritesheets?.TryGetValue(SpritesheetDef.DimensionsToKey(Width, Height), out var ssOut) == true) ? ssOut : null;

            if (SpriteID != spritesheet?.SpriteID)
                jObj.Add("SpriteID", new JValue(SpriteID));
            if (VerticalOffset != spritesheet?.VerticalOffset)
                jObj.Add("VerticalOffset", new JValue(VerticalOffset));
            if (Unknown0x08 != spritesheet?.Unknown0x08)
                jObj.Add("Unknown0x08", new JValue(Unknown0x08));
            if (CollisionSize != spritesheet?.CollisionSize)
                jObj.Add("CollisionSize", new JValue(CollisionSize));
            if (!(Scale >= spritesheet?.Scale - 0.001f && Scale <= spritesheet?.Scale + 0.001f))
                jObj.Add("Scale", new JValue(Scale));

            if (SpriteFrames != null)
                jObj.Add("Frames", JToken.FromObject(SpriteFrames.Select(x => x.ToJToken()).ToArray(), jsonSettings));
            if (SpriteAnimations != null)
                jObj.Add("Animations", JToken.FromObject(SpriteAnimations, jsonSettings));

            return jObj;
        }

        public string SpriteName;

        public int SpriteID;
        public int Width;
        public int Height;
        public int Directions;
        public int PromotionLevel;

        public int VerticalOffset;
        public int Unknown0x08;
        public int CollisionSize;
        public float Scale;

        public SpriteFramesDef[] SpriteFrames;
        public SpriteAnimationsDef[] SpriteAnimations;
    }
}
