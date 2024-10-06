using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X002_Editor.Models.MusicOverride
{
    public class MusicOverrideList : IModelArray<MusicOverride>
    {
        private MusicOverride[] itemssorted;
        private MusicOverride[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load(ScenarioType scenario)
        {
            if (scenario == ScenarioType.Scenario1)
            {
                r = "RSc1/musicOverrideListS1.xml";
            }
            else if (scenario == ScenarioType.Scenario2)
            {
                r = "RSc2/musicOverrideListS2.xml";
            }
            if (scenario == ScenarioType.Scenario3)
            {
                r = "Resources/musicOverrideList.xml";
            }
            else if (scenario == ScenarioType.PremiumDisk)
            {
                r = "RPD/musicOverrideListPD.xml";
            }

            itemssorted = new MusicOverride[0];
            items = new MusicOverride[300]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                MusicOverride[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new MusicOverride[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new MusicOverride[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new MusicOverride(scenario, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].MusicOverrideID] = itemssorted[old.Length];
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

        public MusicOverride[] Models => itemssorted;
    }
}
