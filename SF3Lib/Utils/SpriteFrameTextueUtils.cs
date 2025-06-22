using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using SF3.Models.Structs.CHR;

namespace SF3.Utils {
    public static class SpriteFrameTextueUtils {
        private static Dictionary<string, FrameTextureInfo> s_frameTextureInfosByHash = null;

        public static FrameTextureInfo GetFrameTextureInfoByHash(string hash, int width, int height) {
            LoadFramesByHashTable();
            if (!s_frameTextureInfosByHash.ContainsKey(hash.ToLower()))
                s_frameTextureInfosByHash[hash] = new FrameTextureInfo(hash, "Unknown", width, height, "Unknown");
            return s_frameTextureInfosByHash[hash];
        }

        private static void LoadFramesByHashTable() {
            if (s_frameTextureInfosByHash != null)
                return;
            s_frameTextureInfosByHash = new Dictionary<string, FrameTextureInfo>();

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
                        var hash       = xml.GetAttribute("hash");
                        var sprite     = xml.GetAttribute("sprite");
                        var widthAttr  = xml.GetAttribute("width");
                        var heightAttr = xml.GetAttribute("height");

                        if (hash == null || sprite == null || widthAttr == null || heightAttr == null)
                            continue;

                        int width, height;
                        if (!int.TryParse(widthAttr, out width) || !int.TryParse(heightAttr, out height))
                            continue;

                        var animation = xml.GetAttribute("animation") ?? "";
                        s_frameTextureInfosByHash.Add(hash.ToLower(), new FrameTextureInfo(hash, sprite, width, height, animation));
                    }
                }
            }
        }

        public static void WriteSpriteFramesByHashXML(StreamWriter stream) {
            var frameInfos = s_frameTextureInfosByHash.Values
                .OrderBy(x => x.SpriteName)
                .ThenBy(x => x.Width)
                .ThenBy(x => x.Height)
                .ThenBy(x => x.AnimationName)
                .ThenBy(x => x.TextureHash)
                .ToArray();

            stream.NewLine = "\n";
            stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            stream.WriteLine("<items>");
            foreach (var fi in frameInfos)
                stream.WriteLine($"    <item hash=\"{fi.TextureHash}\" sprite=\"{fi.SpriteName}\" width=\"{fi.Width}\" height=\"{fi.Height}\" animation=\"{fi.AnimationName}\" />");
            stream.WriteLine("</items>");
        }
    }
}
