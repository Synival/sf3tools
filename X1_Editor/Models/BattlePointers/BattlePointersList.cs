using System;
using System.Xml;
using System.IO;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.BattlePointers
{
    public static class BattlePointersList
    {
        private static BattlePointers[] itemssorted;
        private static BattlePointers[] items;

        private static string r = "";

        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadBattlePointersList()
        {
            r = "Resources/BattlePointersList.xml";

            itemssorted = new BattlePointers[0];
            items = new BattlePointers[5]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                BattlePointers[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new BattlePointers[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new BattlePointers[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new BattlePointers(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].BattleID] = itemssorted[old.Length];
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

        public static BattlePointers[] getBattlePointersList()
        {
            return itemssorted;
        }
        public static BattlePointers getBattlePointers(int id)
        {
            return items[id];
        }
    }
}
