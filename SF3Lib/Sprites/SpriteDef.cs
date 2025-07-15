using System.Collections.Generic;
using System.Linq;

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
