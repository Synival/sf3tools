using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SF3.Types;

namespace SF3.Sprites {
    public class AnimationFrameDef {
        public AnimationFrameDef() { }

        public AnimationFrameDef(string[] frameHashes, int duration) {
            FrameHashes = frameHashes ?? new string[0];
            Command     = SpriteAnimationFrameCommandType.Frame;
            Parameter   = duration;
        }

        public AnimationFrameDef(SpriteAnimationFrameCommandType command, int parameter) {
            FrameHashes = (command == SpriteAnimationFrameCommandType.Frame) ? new string[0] : null;
            Command     = command;
            Parameter   = parameter;
        }

        public override string ToString() => $"{Command}_{Parameter}";

        public string[] FrameHashes;

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteAnimationFrameCommandType Command;

        public int Parameter;
    };
}
