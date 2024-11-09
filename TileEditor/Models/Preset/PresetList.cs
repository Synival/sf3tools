using System;
using System.IO;
using System.Xml;
using static STHAEditor.Forms.frmMain;

namespace STHAEditor.Models.Presets {
    public static class PresetList {
        private static Preset[] presetssorted;
        private static Preset[] presets;

        private static string r = "";

        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadPresetList() {
            if (Globals.scenario == 1)
                r = "Resources/scenario1Items.xml";
            if (Globals.scenario == 2)
                r = "Resources/scenario2Items.xml";
            if (Globals.scenario == 3)
                r = "Resources/scenario3Items.xml";
            if (Globals.scenario == 4)
                r = "Resources/PDItems.xml";

            presetssorted = new Preset[0];
            presets = new Preset[300]; //max size 
            try {
                var stream = new FileStream(r, FileMode.Open);

                var settings = new XmlReaderSettings {
                    IgnoreComments = true,
                    IgnoreWhitespace = true
                };
                var xml = XmlTextReader.Create(stream, settings);
                _=xml.Read();
                Preset[] old;
                while (!xml.EOF) {
                    _=xml.Read();
                    if (xml.HasAttributes) {
                        old = new Preset[presetssorted.Length];
                        presetssorted.CopyTo(old, 0);
                        presetssorted = new Preset[old.Length + 1];
                        old.CopyTo(presetssorted, 0);
                        presetssorted[old.Length] = new Preset(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        presets[presetssorted[old.Length].SizeID] = presetssorted[old.Length];
                    }
                }
                stream.Close();
            }
            catch (FileLoadException) {
                return false;
            }
            return true;
        }

        public static Preset[] getPresetList() => presetssorted;
        public static Preset getPreset(int id) => presets[id];
    }
}
