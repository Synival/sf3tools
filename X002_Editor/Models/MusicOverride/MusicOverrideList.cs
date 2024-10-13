﻿using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X002_Editor.Models.MusicOverride
{
    public class MusicOverrideList : ModelArray<MusicOverride>
    {
        public MusicOverrideList(IX002_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private MusicOverride[] items;
        private IX002_FileEditor _fileEditor;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public override bool Load()
        {
            if (Scenario == ScenarioType.Scenario1)
            {
                r = "RSc1/musicOverrideListS1.xml";
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                r = "RSc2/musicOverrideListS2.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                r = "Resources/musicOverrideList.xml";
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                r = "RPD/musicOverrideListPD.xml";
            }

            _models = new MusicOverride[0];
            items = new MusicOverride[300]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                MusicOverride[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new MusicOverride[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new MusicOverride[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new MusicOverride(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[_models[old.Length].MusicOverrideID] = _models[old.Length];
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
