﻿using System;
using System.Xml;
using System.IO;
using SF3.Types;
using SF3.Models;

namespace SF3.IconPointerEditor.Models.ItemIcons
{
    public class ItemIconList : ModelArray<ItemIcon>
    {
        public ItemIconList(IIconPointerFileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private ItemIcon[] models;
        private IIconPointerFileEditor _fileEditor;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public override bool Load()
        {
            if (Scenario == ScenarioType.Scenario1)
            {
                r = "Resources/scenario1Items.xml";
            }
            if (Scenario == ScenarioType.Scenario2)
            {
                r = "Resources/scenario2Items.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                r = "Resources/scenario3Items.xml";
            }
            if (Scenario == ScenarioType.PremiumDisk)
            {
                r = "Resources/PDItems.xml";
            }

            _models = new ItemIcon[0];
            models = new ItemIcon[300]; //max size 
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                ItemIcon[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new ItemIcon[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new ItemIcon[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new ItemIcon(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[_models[old.Length].SizeID] = _models[old.Length];
                    }
                }
            }
            catch (FileLoadException)
            {
                return false;
                //} catch (FileNotFoundException) {
                //  return false;
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
