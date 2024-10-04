using System;
using System.Xml;
using System.IO;
using static SF3.X002_Editor.Forms.frmMain;
using SF3.Models;

namespace SF3.X002_Editor.Models.StatBoost
{
    public class StatList : IModelArray<StatBoost>
    {
        private StatBoost[] itemssorted;
        private StatBoost[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            r = "Resources/statList.xml";

            itemssorted = new StatBoost[0];
            items = new StatBoost[300]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                StatBoost[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new StatBoost[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new StatBoost[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new StatBoost(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].StatID] = itemssorted[old.Length];
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

        public StatBoost[] Models => itemssorted;
    }
}
