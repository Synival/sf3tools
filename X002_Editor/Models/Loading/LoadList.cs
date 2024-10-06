using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X002_Editor.Models.Loading
{
    public class LoadList : IModelArray<Loading>
    {
        private Loading[] itemssorted;
        private Loading[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load(ScenarioType scenario)
        {
            if (scenario == ScenarioType.Scenario1)
            {
                r = "RSc1/loadListS1.xml";
            }
            else if (scenario == ScenarioType.Scenario2)
            {
                r = "RSc2/loadListS2.xml";
            }
            if (scenario == ScenarioType.Scenario3)
            {
                r = "Resources/loadList.xml";
            }
            else if (scenario == ScenarioType.PremiumDisk)
            {
                r = "RPD/loadListPD.xml";
            }

            itemssorted = new Loading[0];
            items = new Loading[300]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Loading[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Loading[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new Loading[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new Loading(scenario, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].LoadID] = itemssorted[old.Length];
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

        public Loading[] Models => itemssorted;
    }
}
