using System;
using System.Xml;
using System.IO;
using static SF3.X002_Editor.Forms.frmMain;

namespace SF3.X002_Editor.Models.WeaponRank
{
    public class WeaponRankList
    {
        private WeaponRank[] itemssorted;
        private WeaponRank[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool loadWeaponRankList()
        {
            r = "Resources/WeaponRankList.xml";

            itemssorted = new WeaponRank[0];
            items = new WeaponRank[5]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                WeaponRank[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new WeaponRank[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new WeaponRank[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new WeaponRank(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].WeaponRankID] = itemssorted[old.Length];
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

        public WeaponRank[] getWeaponRankList()
        {
            return itemssorted;
        }
        public WeaponRank getWeaponRank(int id)
        {
            return items[id];
        }
    }
}
