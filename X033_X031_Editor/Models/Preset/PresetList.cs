using System;
using System.Xml;
using System.IO;
using static SF3.X033_X031_Editor.Forms.frmMain;

namespace SF3.X033_X031_Editor.Models.Presets
{
    public class PresetList
    {
        private Preset[] presetssorted;
        private Preset[] presets;

        private string r = "";

        /// <summary>
        /// Initialises list
        /// </summary>
        /// <returns>'true' on success, 'false' if .xml files do not exist or are in use</returns>
        public bool loadPresetList()
        {
            if (Globals.scenario == 1)
            {
                r = "RSc1/classEquipS1.xml";
            }
            else if (Globals.scenario == 2)
            {
                r = "RSc2/classEquipS2.xml";
            }
            if (Globals.scenario == 3)
            {
                r = "Resources/classEquip.xml";
            }
            else if (Globals.scenario == 4)
            {
                r = "RPD/classEquipPD.xml";
            }

            presetssorted = new Preset[0];
            presets = new Preset[100]; //max size of spellIndexList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Preset[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Preset[presetssorted.Length];
                        presetssorted.CopyTo(old, 0);
                        presetssorted = new Preset[old.Length + 1];
                        old.CopyTo(presetssorted, 0);
                        presetssorted[old.Length] = new Preset(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        presets[presetssorted[old.Length].PresetID] = presetssorted[old.Length];
                    }
                }
                stream.Close();
            }
            catch (FileLoadException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            return true;
        }

        public Preset[] getPresetList()
        {
            return presetssorted;
        }
        public Preset getPreset(int id)
        {
            return presets[id];
        }
    }
}
