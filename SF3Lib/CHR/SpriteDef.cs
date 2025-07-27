using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

                SpriteName     = jObj.TryGetValue("SpriteName", out var spriteName) ? ((string) spriteName) : null;

                SpriteID       = jObj.TryGetValue("SpriteID", out var spriteId) ? ((int) spriteId) : 0;
                Width          = jObj.TryGetValue("Width", out var width) ? ((int) width) : 0;
                Height         = jObj.TryGetValue("Height", out var height) ? ((int) height) : 0;
                Directions     = jObj.TryGetValue("Directions", out var directions) ? ((int) directions) : 0;
                PromotionLevel = jObj.TryGetValue("PromotionLevel", out var promotionLevel) ? ((int?) promotionLevel) : null;

                VerticalOffset = jObj.TryGetValue("VerticalOffset", out var verticalOffset) ? ((int) verticalOffset) : 0;
                Unknown0x08    = jObj.TryGetValue("Unknown0x08", out var unknown0x08) ? ((int) unknown0x08) : 0;
                CollisionSize  = jObj.TryGetValue("CollisionSize", out var collisionSize) ? ((int) collisionSize) : 0;
                Scale          = jObj.TryGetValue("Scale", out var scale) ? ((float?) scale) : 0;

                SpriteFrames = jObj.TryGetValue("SpriteFrames", out var spriteFrames)
                    ? spriteFrames.Select(x => SpriteFramesDef.FromJToken(x)).ToArray()
                    : null;

                SpriteAnimations = jObj.TryGetValue("SpriteAnimations", out var spriteAnimations)
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

            jObj.Add("SpriteName",     new JValue(SpriteName));

            jObj.Add("SpriteID",       new JValue(SpriteID));
            jObj.Add("Width",          new JValue(Width));
            jObj.Add("Height",         new JValue(Height));
            jObj.Add("Directions",     new JValue(Directions));
            if (PromotionLevel.HasValue)
                jObj.Add("PromotionLevel", new JValue(PromotionLevel.Value));

            jObj.Add("VerticalOffset", new JValue(VerticalOffset));
            jObj.Add("Unknown0x08",    new JValue(Unknown0x08));
            jObj.Add("CollisionSize",  new JValue(CollisionSize));
            if (Scale.HasValue)
                jObj.Add("Scale", new JValue(Scale));

            if (SpriteFrames != null)
                jObj.Add("SpriteFrames", JToken.FromObject(SpriteFrames, jsonSettings));
            if (SpriteAnimations != null)
                jObj.Add("SpriteAnimations", JToken.FromObject(SpriteAnimations, jsonSettings));

            return jObj;
        }

        public string SpriteName;

        public int SpriteID;
        public int Width;
        public int Height;
        public int Directions;
        public int? PromotionLevel;

        public int VerticalOffset;
        public int Unknown0x08;
        public int CollisionSize;
        public float? Scale;

        public SpriteFramesDef[] SpriteFrames;
        public SpriteAnimationsDef[] SpriteAnimations;
    }
}
