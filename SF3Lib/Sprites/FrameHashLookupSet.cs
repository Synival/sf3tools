using System;
using System.Collections.Generic;
using System.Linq;
using SF3.Types;

namespace SF3.Sprites {
    public class FrameHashLookupSet : HashSet<FrameHashLookup> {
        public FrameHashLookupSet(string imageHash) {
            ImageHash = imageHash;
        }

        public FrameHashLookupSet(string imageHash, IEnumerable<FrameHashLookup> collection) : base(collection) {
            ImageHash = imageHash;
        }

        private string GetAggregateString(Func<FrameHashLookup, string> lookupFunc)
            => (Count == 0) ? "(Unknown)" : string.Join(" | ", this.Select(x => lookupFunc(x)).OrderBy(x => x).Distinct());

        private T GetUniqueValue<T>(Func<FrameHashLookup, T> lookupFunc, T defaultValue = default) {
            if (Count == 0)
                return defaultValue;

            var firstValue = lookupFunc(this.First());
            if (Count == 1)
                return firstValue;

            if (!this.All(x => lookupFunc(x).Equals(firstValue)))
                return defaultValue;

            return firstValue;
        }

        public string GetAggergateSpriteName() => GetAggregateString(x => x.SpriteName);
        public string GetUniqueSpriteName() => GetUniqueValue(x => x.SpriteName, null);
        public int?   GetUniqueFrameWidth() => GetUniqueValue(x => x.FrameWidth, (int?) null);
        public int?   GetUniqueFrameHeight() => GetUniqueValue(x => x.FrameHeight, (int?) null);
        public string GetAggregateFrameGroupName() => GetAggregateString(x => x.FrameGroupName);
        public string GetUniqueFrameGroupName() => GetUniqueValue(x => x.FrameGroupName, null);
        public SpriteFrameDirection? GetUniqueFrameDirection() => GetUniqueValue(x => x.FrameDirection, (SpriteFrameDirection?) null);

        public readonly string ImageHash;
    }
}
