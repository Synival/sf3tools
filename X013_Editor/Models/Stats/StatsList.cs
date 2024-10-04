using System;
using System.Xml;
using System.IO;
using static SF3.X013_Editor.Forms.frmMain;
using SF3.Models;

namespace SF3.X013_Editor.Models.Stats
{
    public class StatsList : IModelArray<Stat>
    {
        private Stat[] itemssorted;
        private Stat[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            r = "Resources/StatList.xml";

            itemssorted = new Stat[0];
            items = new Stat[256]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Stat[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Stat[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new Stat[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new Stat(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
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

        public Stat[] Models => itemssorted;
    }
}
