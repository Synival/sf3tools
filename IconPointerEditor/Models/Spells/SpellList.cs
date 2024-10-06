﻿using System;
using System.Xml;
using System.IO;
using SF3.Types;
using SF3.Models;

namespace SF3.IconPointerEditor.Models.Spells
{
    public class SpellList : IModelArray<Spell>
    {
        public SpellList(ScenarioType scenario)
        {
            Scenario = scenario;
        }

        public ScenarioType Scenario { get; }

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
                r = "RSc1/spellListS1.xml";
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                r = "RSc2/spellListS2.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                r = "Resources/spellList.xml";
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                r = "RPD/spellListPD.xml";
            }

            spellssorted = new Spell[0];
            spells = new Spell[75]; //max size of spellList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);
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
                        spellssorted[old.Length] = new Spell(Scenario, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        spells[spellssorted[old.Length].SpellID] = spellssorted[old.Length];
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

        public Spell[] Models => spellssorted;
    }
}
