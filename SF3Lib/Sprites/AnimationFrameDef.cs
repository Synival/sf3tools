using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SF3.Types;
using SF3.Utils;

namespace SF3.Sprites {
    public class AnimationFrameDef {
        public AnimationFrameDef() { }

        public AnimationFrameDef(string[] frameHashes, int duration) {
            FrameGroup       = null;
            FrameHashes = frameHashes ?? new string[0];
            Command     = SpriteAnimationFrameCommandType.Frame;
            Parameter   = duration;
        }

        public AnimationFrameDef(SpriteAnimationFrameCommandType command, int parameter) {
            FrameGroup       = null;
            FrameHashes = (command == SpriteAnimationFrameCommandType.Frame) ? new string[0] : null;
            Command     = command;
            Parameter   = parameter;
        }

        public bool ConvertHashesToFrameGroup(Dictionary<string, FrameGroupDef> frameGroups, int frameCount) {
            if (frameGroups == null || FrameHashes == null || FrameHashes.Length != frameCount || FrameHashes.Any(x => x == null))
                return false;

            bool FrameGroupHasHashes(FrameGroupDef fg) {
                return FrameHashes
                    .Select((x, i) => (Hash: x, Dir: CHR_Utils.FrameNumberToSpriteDir(frameCount, i).ToString()))
                    .All(x => fg.Frames.ContainsKey(x.Dir) && fg.Frames[x.Dir].Hash == x.Hash);
            }

            var frameGroup = frameGroups
                .FirstOrDefault(x => FrameGroupHasHashes(x.Value));
            if (frameGroup.Value == null)
                return false;

            FrameHashes = null;
            FrameGroup = frameGroup.Key;
            return true;
        }

        public override string ToString() => (FrameGroup != null)  ? $"{FrameGroup}_{Parameter}" : $"{Command}_{Parameter}";

        [JsonIgnore]
        public bool HasFrame => Command == SpriteAnimationFrameCommandType.Frame && (FrameGroup != null || FrameHashes != null);

        [JsonIgnore]
        public bool HasFullFrame => Command == SpriteAnimationFrameCommandType.Frame && (FrameGroup != null || (FrameHashes != null && FrameHashes.All(x => x != null)));

        public string FrameGroup;
        public string[] FrameHashes;

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteAnimationFrameCommandType Command;

        public int Parameter;
    };
}
