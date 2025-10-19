using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SF3.Sprites {
    public class Formatter {
        public string Format(string text)
            => Format(JToken.Parse(text));

        public string Format(JToken token) {
            using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture)) {
                stringWriter.NewLine = "\n";
                JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
                jsonTextWriter.Formatting = Formatting.Indented;
                WriteIfType(token, jsonTextWriter, JTokenType.Object, () => WriteSprite((JObject) token, jsonTextWriter));
                return stringWriter.ToString();
            }
        }

        public void WriteSprite(JObject sprite, JsonTextWriter writer) {
            WriteObjectImpl(sprite, writer, new Dictionary<string, Action<JProperty>>() {
                { "Spritesheets", (prop) => {
                    WriteIfType(prop.Value, writer, JTokenType.Object, () => WriteSpritesheetDictionary((JObject) prop.Value, writer));
                }}
            });
        }

        public void WriteSpritesheetDictionary(JObject spritesheets, JsonTextWriter writer) {
            WriteObjectImpl(spritesheets, writer, null, (prop) => {
                WriteIfType(prop.Value, writer, JTokenType.Object, () => WriteSpritesheet((JObject) prop.Value, writer));
            });
        }

        public void WriteSpritesheet(JObject spritesheet, JsonTextWriter writer) {
            WriteObjectImpl(spritesheet, writer, new Dictionary<string, Action<JProperty>>() {
                { "FrameGroups", (prop) => {
                    WriteIfType(prop.Value, writer, JTokenType.Object, () => WriteFrameGroupDictionary((JObject) prop.Value, writer));
                }},
                { "AnimationByDirections", (prop) => {
                    WriteIfType(prop.Value, writer, JTokenType.Object, () => WriteAnimationByDirections((JObject) prop.Value, writer));
                }}
            });
        }

        public void WriteFrameGroupDictionary(JObject animations, JsonTextWriter writer) {
            WriteObjectImpl(animations, writer, null,
                (prop) => WriteCondensed(prop.Value, writer)
            );
        }

        public void WriteAnimationByDirections(JObject animationsByDir, JsonTextWriter writer) {
            WriteObjectImpl(animationsByDir, writer, null,
                (prop) => WriteIfType(prop.Value, writer, JTokenType.Object, () => WriteAnimationDictionary((JObject) prop.Value, writer))
            );
        }

        public void WriteAnimationDictionary(JObject animations, JsonTextWriter writer) {
            WriteObjectImpl(animations, writer, null,
                (prop) => WriteIfType(prop.Value, writer, JTokenType.Array, () => WriteAnimation((JArray) prop.Value, writer))
            );
        }

        public void WriteAnimation(JArray animations, JsonTextWriter writer) {
            writer.WriteStartArray();
            foreach (var value in animations)
                WriteCondensed(value, writer);
            writer.WriteEndArray();
        }

        private void WriteCondensed(JToken token, JsonTextWriter writer) {
            using (var stringWriter = new StringWriter()) {
                stringWriter.NewLine = "\n";
                var jsonWriter = new JsonTextWriter(stringWriter);
                jsonWriter.Formatting = Formatting.Indented;
                token.WriteTo(jsonWriter);
                var text = stringWriter.ToString();
                writer.WriteRawValue(string.Join(" ", text.Split('\n').Select(x => x.Trim())));
            }
        }

        private void WriteIfType(JToken token, JsonTextWriter writer, JTokenType type, Action action, Action defaultAction = null)
            => WriteByType(token, writer, new Dictionary<JTokenType, Action> {{ type, action }}, defaultAction);

        private void WriteByType(JToken token, JsonTextWriter writer, Dictionary<JTokenType, Action> actions, Action defaultAction = null) {
            if (actions != null && actions.TryGetValue(token.Type, out var action))
                action();
            else if (defaultAction != null)
                defaultAction();
            else
                token.WriteTo(writer);
        }

        private void WriteObjectImpl(JObject token, JsonTextWriter writer, Dictionary<string, Action<JProperty>> propertyActions, Action<JProperty> defaultPropertyAction = null) {
            writer.WriteStartObject();
            foreach (var prop in token.Properties()) {
                writer.WritePropertyName(prop.Name);
                if (propertyActions != null && propertyActions.TryGetValue(prop.Name, out var action))
                    action(prop);
                else if (defaultPropertyAction != null)
                    defaultPropertyAction(prop);
                else
                    prop.Value.WriteTo(writer);
            }
            writer.WriteEndObject();
        }

        private void WriteArrayImpl(JArray token, JsonTextWriter writer, Action<JToken> valueAction) {
            writer.WriteStartArray();
            foreach (var value in token.Values())
                valueAction(value);
            writer.WriteEndArray();
        }
    }
}
