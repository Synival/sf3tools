﻿using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X013_Editor.Models.Spells
{
    public class SpellList : IModelArray<Spell>
    {
        public SpellList(IX013_FileEditor fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX013_FileEditor _fileEditor;
        public ScenarioType Scenario => _fileEditor.Scenario;

        private Spell[] spellssorted;
        private Spell[] spells;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            if (Scenario == ScenarioType.Scenario1)
            {
                r = "RSc1/charactersS1.xml";
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                r = "RSc2/charactersS2.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                r = "Resources/characters.xml";
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                r = "RPD/charactersPD.xml";
            }

            spellssorted = new Spell[0];
            spells = new Spell[120]; //max size of spellList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Spell[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Spell[spellssorted.Length];
                        spellssorted.CopyTo(old, 0);
                        spellssorted = new Spell[old.Length + 1];
                        old.CopyTo(spellssorted, 0);
                        spellssorted[old.Length] = new Spell(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        spells[spellssorted[old.Length].SpellID] = spellssorted[old.Length];
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

        public Spell[] Models => spellssorted;
    }
}
