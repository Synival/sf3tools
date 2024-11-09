using System;
using System.IO;
using System.Xml;
using static STHAEditor.Forms.frmMain;

namespace STHAEditor.Models.Items {
    public static class ItemList {
        private static Item[] itemssorted;
        private static Item[] items;

        private static string r = "";

        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadItemList() {
            if (Globals.scenario == 1)
                r = "Resources/scenario1Spells.xml";
            if (Globals.scenario == 2)
                r = "Resources/scenario2Spells.xml";
            if (Globals.scenario == 3)
                r = "Resources/scenario3Spells.xml";
            if (Globals.scenario == 4)
                r = "Resources/PDSpells.xml";

            itemssorted = new Item[0];
            items = new Item[256]; //max size of itemList
            try {
                var stream = new FileStream(r, FileMode.Open);

                var settings = new XmlReaderSettings {
                    IgnoreComments = true,
                    IgnoreWhitespace = true
                };
                var xml = XmlTextReader.Create(stream, settings);
                _=xml.Read();
                Item[] old;
                while (!xml.EOF) {
                    _=xml.Read();
                    if (xml.HasAttributes) {
                        old = new Item[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new Item[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new Item(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].ID] = itemssorted[old.Length];
                    }
                }
                stream.Close();
            }
            catch (FileLoadException) {
                return false;
            }
            catch (FileNotFoundException) {
                return false;
            }
            return true;
        }

        public static Item[] getItemList() => itemssorted;
        public static Item getItem(int id) => items[id];
    }
}
