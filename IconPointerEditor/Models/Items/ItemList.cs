using System;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using SF3.Types;
using SF3.Models;

namespace SF3.IconPointerEditor.Models.Items
{
    public class ItemList : IModelArray<Item>
    {
        public ItemList(ISF3FileEditor fileEditor)
        {
            _fileEditor = fileEditor;
        }

        public ScenarioType Scenario => _fileEditor.Scenario;

        private Item[] itemssorted;
        private Item[] items;
        private ISF3FileEditor _fileEditor;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>'true' on success, 'false' if .xml files do not exist or are in use</returns>
        public bool Load()
        {
            if (Scenario == ScenarioType.Scenario1)
            {
                r = "Resources/scenario1Spells.xml";
            }
            if (Scenario == ScenarioType.Scenario2)
            {
                r = "Resources/scenario2Spells.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                r = "Resources/scenario3Spells.xml";
            }
            if (Scenario == ScenarioType.PremiumDisk)
            {
                r = "Resources/PDSpells.xml";
            }

            itemssorted = new Item[0];
            items = new Item[256]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Item[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Item[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new Item[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new Item(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].ID] = itemssorted[old.Length];
                        //MessageBox.Show("" + _fileEditor.GetDouble(itemssorted[itemssorted.Length - 1].Address));
                    }
                }
            }
            catch (FileLoadException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return true;
        }

        public Item[] Models => itemssorted;
    }
}
