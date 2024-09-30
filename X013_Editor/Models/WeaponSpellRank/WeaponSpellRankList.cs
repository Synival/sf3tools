using System;
using System.Xml;
using System.IO;
using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.WeaponSpellRank
{
    public static class WeaponSpellRankList
    {
        private static WeaponSpellRank[] itemssorted;
        private static WeaponSpellRank[] items;


        private static string r = "";




        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadWeaponSpellRankList()
        {




            r = "Resources/WeaponSpellRankList.xml";



            itemssorted = new WeaponSpellRank[0];
            items = new WeaponSpellRank[4]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                WeaponSpellRank[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new WeaponSpellRank[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new WeaponSpellRank[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new WeaponSpellRank(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].WeaponSpellRankID] = itemssorted[old.Length];
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

        public static WeaponSpellRank[] getWeaponSpellRankList()
        {
            return itemssorted;
        }
        public static WeaponSpellRank getWeaponSpellRank(int id)
        {
            return items[id];
        }
    }
}
