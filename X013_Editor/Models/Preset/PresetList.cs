using System;
//using System.Text;
using System.Xml;
using System.IO;
using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.Presets
{
    public static class PresetList
    {
        private static Preset[] presetssorted;
        private static Preset[] presets;

        private static string r = "";

        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadPresetList()
        {

                r = "Resources/ExpList.xml";



            presetssorted = new Preset[0];
            presets = new Preset[1]; //max size of spellIndexList
            try {
                FileStream stream = new FileStream(r, FileMode.Open);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Preset[] old;
                while (!xml.EOF) {
                    xml.Read();
                    if (xml.HasAttributes) {
                        old = new Preset[presetssorted.Length];
                        presetssorted.CopyTo(old, 0);
                        presetssorted = new Preset[old.Length + 1];
                        old.CopyTo(presetssorted, 0);
                        presetssorted[old.Length] = new Preset(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        presets[presetssorted[old.Length].PresetID] = presetssorted[old.Length];
                    }
                }
                stream.Close();
            } catch (FileLoadException) {
                return false;
            } catch (FileNotFoundException) {
                return false;
            }
            return true;
        }

        public static Preset[] getPresetList()
        {
            return presetssorted;
        }
        public static Preset getPreset(int id)
        {
            return presets[id];
        }
    }
}
