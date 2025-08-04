using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.Sprites;
using SF3.Types;
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
                Directions     = jObj.TryGetValue("Directions",     out var directions)     ? SpriteDirectionCountTypeExtensions.FromJToken(directions) : SpriteDirectionCountType.Four;
                PromotionLevel = jObj.TryGetValue("PromotionLevel", out var promotionLevel) ? ((int) promotionLevel) : 0;

                var spriteDef = SpriteUtils.GetSpriteDef(SpriteName);

                Width          = jObj.TryGetValue("Width",          out var width)          ? ((int) width)          : spriteDef?.Width  ?? 0;
                Height         = jObj.TryGetValue("Height",         out var height)         ? ((int) height)         : spriteDef?.Height ?? 0;

                var spritesheet = (spriteDef?.Spritesheets?.TryGetValue(Spritesheet.DimensionsToKey(Width, Height), out var ssOut) == true) ? ssOut : null;

                SpriteID       = jObj.TryGetValue("SpriteID",       out var spriteId)       ? ((int) spriteId)       : spritesheet?.SpriteID       ?? 0;
                VerticalOffset = jObj.TryGetValue("VerticalOffset", out var verticalOffset) ? ((int) verticalOffset) : spritesheet?.VerticalOffset ?? 0;
                Unknown0x08    = jObj.TryGetValue("Unknown0x08",    out var unknown0x08)    ? ((int) unknown0x08)    : spritesheet?.Unknown0x08    ?? 0;
                CollisionSize  = jObj.TryGetValue("CollisionSize",  out var collisionSize)  ? ((int) collisionSize)  : spritesheet?.CollisionSize  ?? 0;
                Scale          = jObj.TryGetValue("Scale",          out var scale)          ? ((float) scale)        : spritesheet?.Scale          ?? 0;

                if (jObj.TryGetValue("Frames", out var frames))
                    FrameGroupsForSpritesheets = new FrameGroupsForSpritesheet[] { FrameGroupsForSpritesheet.FromJToken(frames) };
                else if (jObj.TryGetValue("FramesBySprite", out var spriteFrames) && spriteFrames.Type == JTokenType.Array)
                    FrameGroupsForSpritesheets = spriteFrames.Select(x => FrameGroupsForSpritesheet.FromJToken(x)).ToArray();

                if (jObj.TryGetValue("Animations", out var animations))
                    AnimationsForSpritesheetAndDirections = new AnimationsForSpritesheetAndDirection[] { AnimationsForSpritesheetAndDirection.FromJToken(animations) };
                else if (jObj.TryGetValue("AnimationsBySprite", out var spriteAnimations) && spriteAnimations.Type == JTokenType.Array)
                    AnimationsForSpritesheetAndDirections = spriteAnimations.Select(x => AnimationsForSpritesheetAndDirection.FromJToken(x)).ToArray();

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

            jObj.Add("Name", new JValue(SpriteName));

            if (Directions != SpriteDirectionCountType.Four)
                jObj.Add("Directions", Directions.ToJToken());
            if (PromotionLevel != 0)
                jObj.Add("PromotionLevel", new JValue(PromotionLevel));

            var spriteDef = SpriteUtils.GetSpriteDef(SpriteName);
            var spritesheet = (spriteDef?.Spritesheets?.TryGetValue(Spritesheet.DimensionsToKey(Width, Height), out var ssOut) == true) ? ssOut : null;

            if (Width != spriteDef?.Width || Height != spriteDef?.Height) {
                jObj.Add("Width", new JValue(Width));
                jObj.Add("Height", new JValue(Height));
            }

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

            if (FrameGroupsForSpritesheets != null) {
                if (FrameGroupsForSpritesheets.Length == 1)
                    jObj.Add("Frames", FrameGroupsForSpritesheets[0].ToJToken());
                else
                    jObj.Add("FramesBySprite", JToken.FromObject(FrameGroupsForSpritesheets.Select(x => x.ToJToken()).ToArray(), jsonSettings));
            }

            if (AnimationsForSpritesheetAndDirections != null) {
                if (AnimationsForSpritesheetAndDirections.Length == 1)
                    jObj.Add("Animations", AnimationsForSpritesheetAndDirections[0].ToJToken());
                else
                    jObj.Add("AnimationsBySprite", JToken.FromObject(AnimationsForSpritesheetAndDirections.Select(x => x.ToJToken()), jsonSettings));
            }

            var jStr = jObj.ToString();
            return jObj;
        }

        public string SpriteName;

        public int SpriteID;
        public int Width;
        public int Height;
        public SpriteDirectionCountType Directions;
        public int PromotionLevel;

        public int VerticalOffset;
        public int Unknown0x08;
        public int CollisionSize;
        public float Scale;

        public FrameGroupsForSpritesheet[] FrameGroupsForSpritesheets;
        public AnimationsForSpritesheetAndDirection[] AnimationsForSpritesheetAndDirections;
    }
}
