using System.Linq;
using Newtonsoft.Json.Linq;

namespace SF3.CHR {
    public class SpriteDef {
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

        /// <summary>
        /// Deserializes a JSON object of a SpriteDef.
        /// </summary>
        /// <param name="json">SpriteDef in JSON format as a string.</param>
        /// <returns>A new SpriteDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteDef FromJSON(string json)
            => FromJToken(JToken.Parse(json));

        /// <summary>
        /// Deserializes a JSON object of a SpriteDef.
        /// </summary>
        /// <param name="jToken">SpriteDef as a JToken.</param>
        /// <returns>A new SpriteDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteDef FromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return null;

            try {
                var jObj = (JObject) jToken;
                var newDef = new SpriteDef();

                newDef.SpriteName     = jObj.TryGetValue("SpriteName", out var spriteName) ? ((string) spriteName) : null;

                newDef.SpriteID       = jObj.TryGetValue("SpriteID", out var spriteId) ? ((int) spriteId) : 0;
                newDef.Width          = jObj.TryGetValue("Width", out var width) ? ((int) width) : 0;
                newDef.Height         = jObj.TryGetValue("Height", out var height) ? ((int) height) : 0;
                newDef.Directions     = jObj.TryGetValue("Directions", out var directions) ? ((int) directions) : 0;
                newDef.PromotionLevel = jObj.TryGetValue("PromotionLevel", out var promotionLevel) ? ((int?) promotionLevel) : null;

                newDef.VerticalOffset = jObj.TryGetValue("VerticalOffset", out var verticalOffset) ? ((int) verticalOffset) : 0;
                newDef.Unknown0x08    = jObj.TryGetValue("Unknown0x08", out var unknown0x08) ? ((int) unknown0x08) : 0;
                newDef.CollisionSize  = jObj.TryGetValue("CollisionSize", out var collisionSize) ? ((int) collisionSize) : 0;
                newDef.Scale          = jObj.TryGetValue("Scale", out var scale) ? ((float?) scale) : 0;

                newDef.SpriteFrames = jObj.TryGetValue("SpriteFrames", out var spriteFrames)
                    ? spriteFrames.Select(x => SpriteFramesDef.FromJToken(x)).ToArray()
                    : null;

                newDef.SpriteAnimations = jObj.TryGetValue("SpriteAnimations", out var spriteAnimations)
                    ? spriteAnimations.Select(x => SpriteAnimationsDef.FromJToken(x)).ToArray()
                    : null;

                return newDef;
            }
            catch {
                return null;
            }
        }
    }
}
