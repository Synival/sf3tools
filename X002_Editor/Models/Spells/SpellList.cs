using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X002_Editor.Models.Spells
{
    public class SpellList : IModelArray<Spell>
    {
        public SpellList(IFileEditor fileEditor, ScenarioType scenario)
        {
            _fileEditor = fileEditor;
            Scenario = scenario;
        }

        private IFileEditor _fileEditor;
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
            spells = new Spell[78]; //max size of spellList. 
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
                        spellssorted[old.Length] = new Spell(_fileEditor, Scenario, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
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
