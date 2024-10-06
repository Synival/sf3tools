using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X019_Editor.Models.Presets
{
    public class PresetList : IModelArray<Preset>
    {
        public PresetList(ScenarioType scenario)
        {
            Scenario = scenario;
        }

        public ScenarioType Scenario { get; }

        private Preset[] presetssorted;
        private Preset[] presets;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            if (Scenario == ScenarioType.Scenario1)
            {
                r = "RSc1/spellIndexListS1.xml";
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                r = "RSc2/spellIndexListS2.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                r = "Resources/spellIndexList.xml";
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                r = "RPD/spellListIndexPD.xml";
            }

            presetssorted = new Preset[0];
            presets = new Preset[31]; //max size of spellIndexList
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
                        presetssorted[old.Length] = new Preset(Scenario, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
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

        public Preset[] Models => presetssorted;
    }
}
