﻿using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X019_Editor.Models.Monsters
{
    public class MonsterList : IModelArray<Monster>
    {
        public MonsterList(IX019_FileEditor fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX019_FileEditor _fileEditor;
        public ScenarioType Scenario => _fileEditor.Scenario;

        private Monster[] modelsSorted;
        private Monster[] models;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            if (Scenario == ScenarioType.Scenario1)
            {
                r = "RSc1/X019List.xml";
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                r = "RSc2/X019List.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                r = "Resources/X019List.xml";
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                r = "RPD/X019List.xml";
            }
            else if (Scenario == ScenarioType.Other)
            {
                r = "RPDX44/X044List.xml";
            }

            modelsSorted = new Monster[0];
            models = new Monster[256]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Monster[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Monster[modelsSorted.Length];
                        modelsSorted.CopyTo(old, 0);
                        modelsSorted = new Monster[old.Length + 1];
                        old.CopyTo(modelsSorted, 0);
                        modelsSorted[old.Length] = new Monster(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[modelsSorted[old.Length].ID] = modelsSorted[old.Length];
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

        public Monster[] Models => modelsSorted;
    }
}