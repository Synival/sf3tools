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

        public void WriteFrameGroupDictionary(JObject frameGroups, JsonTextWriter writer) {
            writer.WriteStartObject();

            // Gather value types and their condensed value, if desired.
            var properties = frameGroups.Properties().ToArray();
            var condensedValues = new string[properties.Length];
            var valueTypes = new FrameGroupType[properties.Length];
            var maxPropLen = 0;
            for (int i = 0; i < properties.Length; i++) {
                var prop = properties[i];
                var value = prop.Value;
                var valueType = GetFrameGroupType(value);
                valueTypes[i] = valueType;

                var shouldCondense = valueType == FrameGroupType.SimpleFrameGroup;
                if (shouldCondense) {
                    condensedValues[i] = CondensedToken(value);
                    maxPropLen = Math.Max(maxPropLen, prop.Name.Length);
                }
                else
                    condensedValues[i] = null;
            }

            AlignSubstrings(condensedValues, "\"SpritesheetX\":", (str, i) => valueTypes[i] == FrameGroupType.SimpleFrameGroup);
            AlignSubstrings(condensedValues, "\"SpritesheetY\":", (str, i) => valueTypes[i] == FrameGroupType.SimpleFrameGroup);

            // Write the values: condensed if desired, otherwise as-is.
            var index = 0;
            for (int i = 0; i < properties.Length; i++) {
                var prop = properties[i];
                writer.WritePropertyName(prop.Name);

                var condensedValue = condensedValues[index];
                if (condensedValue != null) {
                    if (prop.Name.Length < maxPropLen)
                        writer.WriteRaw(new string(' ', maxPropLen - prop.Name.Length));
                    writer.WriteRawValue(condensedValue);
                }
                else
                    prop.Value.WriteTo(writer);
                index++;
            }

            writer.WriteEndObject();
        }

        private enum FrameGroupType {
            Other,
            SimpleFrameGroup,
        }

        private FrameGroupType GetFrameGroupType(JToken token) {
            if (token.Type != JTokenType.Object)
                return FrameGroupType.Other;

            var obj = (JObject) token;
            var properties = obj.Properties().ToArray();

            if (properties.Length == 3 && properties[0].Name == "Directions" && properties[1].Name == "SpritesheetX" && properties[2].Name == "SpritesheetY")
                return FrameGroupType.SimpleFrameGroup;

            return FrameGroupType.Other;
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

        public void WriteAnimation(JArray animation, JsonTextWriter writer) {
            writer.WriteStartArray();

            // Gather the list of condensed values to write, and some alignments that need to be made.
            var condensedValues = new List<string>();
            var valueTypes = new List<AnimationFrameType>();
            foreach (var value in animation) {
                var frameType = GetAnimationFrameType(value);
                valueTypes.Add(frameType);

                var shouldCondense = frameType == AnimationFrameType.SimpleFrame || frameType == AnimationFrameType.Command;
                if (shouldCondense)
                    condensedValues.Add(CondensedToken(value));
                else
                    condensedValues.Add(null);
            }

            var condensedValuesArray = condensedValues.ToArray();
            var valueTypesArray = valueTypes.ToArray();
            AlignSubstrings(condensedValuesArray, "\"Direction\":", (str, i) => valueTypesArray[i] == AnimationFrameType.SimpleFrame);
            AlignSubstrings(condensedValuesArray, "\"Duration\":", (str, i) => valueTypesArray[i] == AnimationFrameType.SimpleFrame);

            // Write the values: condensed if desired, otherwise as-is.
            var index = 0;
            foreach (var value in animation) {
                var condensedValue = condensedValuesArray[index];
                if (condensedValue != null)
                    writer.WriteRawValue(condensedValue);
                else
                    value.WriteTo(writer);
                index++;
            }

            writer.WriteEndArray();
        }

        private void AlignSubstrings(string[] strings, string substring, Func<string, int, bool> predicate = null) {
            var max = strings.Max(x => (x == null) ? -1 : x.IndexOf(substring));
            for (int i = 0; i < strings.Length; i++) {
                var str = strings[i];
                if (str == null)
                    continue;

                var substrIndex = str.IndexOf(substring);
                if (substrIndex < 0 || substrIndex == max)
                    continue;

                if (predicate != null && !predicate(str, i))
                    continue;

                strings[i] = str.Substring(0, substrIndex) + new string(' ', max - substrIndex) + str.Substring(substrIndex);
            }
        }

        private enum AnimationFrameType {
            Other,
            SimpleFrame,
            Command
        }

        private AnimationFrameType GetAnimationFrameType(JToken token) {
            if (token.Type != JTokenType.Object)
                return AnimationFrameType.Other;

            var obj = (JObject) token;
            var properties = obj.Properties().ToArray();

            if (properties.Length >= 1 && properties[0].Name == "Command")
                return AnimationFrameType.Command;
            if (properties.Length == 2 && properties[0].Name == "Frame" && properties[1].Name == "Duration") {
                if (properties[0].Value.Type == JTokenType.String)
                    return AnimationFrameType.SimpleFrame;
                else if (properties[0].Value.Type == JTokenType.Object) {
                    var frameObj = (JObject) properties[0].Value;
                    if (frameObj.Properties().Count() == 1)
                        return AnimationFrameType.SimpleFrame;
                }
            }

            return AnimationFrameType.Other;
        }

        private string CondensedToken(JToken token) {
            using (var stringWriter = new StringWriter()) {
                stringWriter.NewLine = "\n";
                var jsonWriter = new JsonTextWriter(stringWriter);
                jsonWriter.Formatting = Formatting.Indented;
                token.WriteTo(jsonWriter);
                var text = stringWriter.ToString();
                return string.Join(" ", text.Split('\n').Select(x => x.Trim()));
            }
        }

        private void WriteCondensed(JToken token, JsonTextWriter writer)
            => writer.WriteRawValue(CondensedToken(token));

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
