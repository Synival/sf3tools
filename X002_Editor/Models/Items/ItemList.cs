using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X002_Editor.Models.Items
{
    public class ItemList : ModelArray<Item>
    {
        public ItemList(IX002_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario1)
            {
                _resourceFile = "RSc1/itemListS1.xml";
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                _resourceFile = "RSc2/itemListS2.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                _resourceFile = "Resources/itemList.xml";
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                _resourceFile = "RPD/itemListPD.xml";
            }
        }

        private string _resourceFile;
        private IX002_FileEditor _fileEditor;
        private Item[] items;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new Item[0];
            items = new Item[300]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

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
                        old = new Item[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new Item[old.Length + 1];
                        old.CopyTo(_models, 0);

                        _models[old.Length] = new Item(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[_models[old.Length].ID] = _models[old.Length];
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
    }
}
