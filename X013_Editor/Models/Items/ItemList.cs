﻿using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X013_Editor.Models.Items
{
    public class ItemList : IModelArray<Item>
    {
        public ItemList(IX013_FileEditor fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX013_FileEditor _fileEditor;
        public ScenarioType Scenario => _fileEditor.Scenario;

        private Item[] itemssorted;
        private Item[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            if (Scenario == ScenarioType.Scenario1)
            {
                r = "RSc1/SpecialListS1.xml";
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                r = "RSc2/SpecialListS2.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                r = "Resources/SpecialList.xml";
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                r = "RPD/SpecialListPD.xml";
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
