using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X002_Editor.Models.Spells
{
    public class SpellList : ModelArray<Spell>
    {
        public SpellList(IX002_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario1)
            {
                _resourceFile = "RSc1/spellListS1.xml";
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                _resourceFile = "RSc2/spellListS2.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                _resourceFile = "Resources/spellList.xml";
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                _resourceFile = "RPD/spellListPD.xml";
            }
        }

        private string _resourceFile;
        private IX002_FileEditor _fileEditor;
        private Spell[] spells;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public override bool Load()
        {
            _models = new Spell[0];
            spells = new Spell[78]; //max size of spellList. 
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);
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
                        old = new Spell[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new Spell[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new Spell(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        spells[_models[old.Length].SpellID] = _models[old.Length];
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
