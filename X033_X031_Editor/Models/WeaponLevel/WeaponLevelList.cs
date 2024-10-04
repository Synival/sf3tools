using System;
using System.Xml;
using System.IO;
using static SF3.X033_X031_Editor.Forms.frmMain;

namespace SF3.X033_X031_Editor.Models.WeaponLevel
{
    public class WeaponLevelList
    {
        private WeaponLevel[] itemssorted;
        private WeaponLevel[] items;

        private string r = "";

        /// <summary>
        /// Initialises list
        /// </summary>
        /// <returns>'true' on success, 'false' if .xml files do not exist or are in use</returns>
        public bool loadWeaponLevelList()
        {
            r = "Resources/WeaponLevel.xml";

            itemssorted = new WeaponLevel[0];
            items = new WeaponLevel[2]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                WeaponLevel[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new WeaponLevel[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new WeaponLevel[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new WeaponLevel(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].WeaponLevelID] = itemssorted[old.Length];
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

        public WeaponLevel[] getWeaponLevelList()
        {
            return itemssorted;
        }
        public WeaponLevel getWeaponLevel(int id)
        {
            return items[id];
        }
    }
}
