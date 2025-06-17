using System.Collections.Generic;
using System.IO;
using System.Xml;
using SF3.Models.Structs.CHR;

namespace SF3.Utils {
    public static class SpriteFrameTextueUtils {
        private static Dictionary<string, FrameTextueInfo> s_frameTextureInfosByHash = null;

        public static FrameTextueInfo GetFrameTextureInfoByHash(string hash) {
            LoadFramesByHashTable();
            if (!s_frameTextureInfosByHash.ContainsKey(hash.ToLower()))
                s_frameTextureInfosByHash[hash] = new FrameTextueInfo(hash, "Unknown", "Unknown");
            return s_frameTextureInfosByHash[hash];
        }

        private static void LoadFramesByHashTable() {
            if (s_frameTextureInfosByHash != null)
                return;
            s_frameTextureInfosByHash = new Dictionary<string, FrameTextueInfo>();

            using (var stream = new FileStream(CommonLib.Utils.ResourceUtils.ResourceFile("SpriteFramesByHash.xml"), FileMode.Open, FileAccess.Read)) {
                var settings = new XmlReaderSettings {
                    IgnoreComments = true,
                    IgnoreWhitespace = true
                };

                var xml = XmlReader.Create(stream, settings);
                _ = xml.Read();

                var nameDict = new Dictionary<int, string>();
                while (!xml.EOF) {
                    _ = xml.Read();
                    if (xml.HasAttributes) {
                        var hash      = xml.GetAttribute("hash");
                        var sprite    = xml.GetAttribute("sprite");
                        var animation = xml.GetAttribute("animation");

                        if (hash != null && sprite != null && animation != null)
                            s_frameTextureInfosByHash.Add(hash.ToLower(), new FrameTextueInfo(hash, sprite, animation));
                    }
                }
            }
        }
    }
}
