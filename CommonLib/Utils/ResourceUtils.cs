using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CommonLib.Utils {
    public static class ResourceUtils {

        /// <summary>
        /// Parses an XML file and returns a Dictionary of values (attribute "value") with their names (attribute "name").
        /// </summary>
        /// <param name="filename">XML file to parse.</param>
        /// <returns>a Dictionary of values (attribute "value") with their names (attribute "name").</returns>
        public static Dictionary<int, string> GetValueNameDictionaryFromXML(string filename) {
            var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);

            var settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            var xml = XmlReader.Create(stream, settings);
            xml.Read();

            var nameDict = new Dictionary<int, string>();
            while (!xml.EOF) {
                xml.Read();
                if (xml.HasAttributes) {
                    string valueStr = xml.GetAttribute("value");
                    string name = xml.GetAttribute("name");

                    if (valueStr != null && name != null)
                        nameDict.Add(Convert.ToInt32(valueStr, 16), name);
                }
            }

            return nameDict;
        }
    }
}
