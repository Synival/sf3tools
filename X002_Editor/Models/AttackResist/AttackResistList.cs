using System;
using System.Xml;
using System.IO;
using static SF3.X002_Editor.Forms.frmMain;

namespace SF3.X002_Editor.Models.AttackResist
{
    public class AttackResistList
    {
        private AttackResist[] itemssorted;
        private AttackResist[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool loadAttackResistList()
        {
            r = "Resources/AttackResistList.xml";

            itemssorted = new AttackResist[0];
            items = new AttackResist[2]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                AttackResist[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new AttackResist[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new AttackResist[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new AttackResist(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].AttackResistID] = itemssorted[old.Length];
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

        public AttackResist[] getAttackResistList()
        {
            return itemssorted;
        }
        public AttackResist getAttackResist(int id)
        {
            return items[id];
        }
    }
}
