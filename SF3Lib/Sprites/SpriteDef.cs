using System.Collections.Generic;
using System.Linq;
using SF3.Utils;

namespace SF3.Sprites {
    public class SpriteDef {
        public SpriteDef() { }

        public SpriteDef(string name, UniqueFrameDef[] frames, UniqueAnimationDef[] animations) {
            Name = name;
            Spritesheets = frames
                .OrderBy(x => x.Width)
                .ThenBy(x => x.Height)
                .OrderBy(x => x.FrameName)
                .ThenBy(x => x.Direction)
                .GroupBy(x => SpritesheetDef.DimensionsToKey(x.Width, x.Height))
                .ToDictionary(x => x.Key, x => new SpritesheetDef(
                    x.ToArray(),
                    animations.Where(y => y.Width == x.First().Width && y.Height == x.First().Height).ToArray()
                ));

            foreach (var spritesheet in Spritesheets.Values)
                foreach (var variant in spritesheet.AnimationByDirections)
                    foreach (var animation in variant.Value.Animations.Values)
                        foreach (var aniFrame in animation.AnimationFrames)
                            aniFrame.ConvertFrameHashes(spritesheet.FrameGroups);
        }

        public SpriteDef(string name, StandaloneFrameDef[] frames, UniqueAnimationDef[] animations) {
            Name = name;
            Spritesheets = frames
                .OrderBy(x => x.Width)
                .ThenBy(x => x.Height)
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Direction)
                .GroupBy(x => SpritesheetDef.DimensionsToKey(x.Width, x.Height))
                .ToDictionary(x => x.Key, x => new SpritesheetDef(
                    x.ToArray(),
                    animations.Where(y => y.Width == x.First().Width && y.Height == x.First().Height).ToArray()
                ));
        }

        public override string ToString() => Name;

        public string Name;
        public Dictionary<string, SpritesheetDef> Spritesheets;
    }
}
