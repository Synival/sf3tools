using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CommonLib.Utils {
    public static class ResourceUtils {
        /// <summary>
        /// Returns the relative path of a resource file.
        /// </summary>
        /// <param name="resourceName">The name of the resource file without its path.</param>
        /// <returns>A string with a relative path with file for the resource.</returns>
        public static string ResourceFile(string resourceName)
            => "Resources/" + resourceName;

        /// <summary>
        /// Parses an XML file and returns a Dictionary of values (attribute "value") with their names (attribute "name").
        /// </summary>
        /// <param name="filename">XML file to parse.</param>
        /// <returns>a Dictionary of values (attribute "value") with their names (attribute "name").</returns>
        public static Dictionary<int, string> GetValueNameDictionaryFromXML(string filename) {
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read)) {
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
                        var valueStr = xml.GetAttribute("value");
                        var name = xml.GetAttribute("name");

                        if (valueStr != null && name != null)
                            nameDict.Add(Convert.ToInt32(valueStr, 16), name);
                    }
                }
                return nameDict;
            }
        }
    }
}
