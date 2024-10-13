using System;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using SF3.Types;
using SF3.Models;

namespace SF3.IconPointerEditor.Models.SpellIcons
{
    public class SpellIconList : ModelArray<SpellIcon>
    {
        public SpellIconList(IIconPointerFileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario1)
            {
                _resourceFile = "Resources/scenario1Spells.xml";
            }
            if (Scenario == ScenarioType.Scenario2)
            {
                _resourceFile = "Resources/scenario2Spells.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                _resourceFile = "Resources/scenario3Spells.xml";
            }
            if (Scenario == ScenarioType.PremiumDisk)
            {
                _resourceFile = "Resources/PDSpells.xml";
            }
        }

        private string _resourceFile;
        private SpellIcon[] models;
        private IIconPointerFileEditor _fileEditor;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>'true' on success, 'false' if .xml files do not exist or are in use</returns>
        public override bool Load()
        {
            _models = new SpellIcon[0];
            models = new SpellIcon[256]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                SpellIcon[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new SpellIcon[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new SpellIcon[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new SpellIcon(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[_models[old.Length].ID] = _models[old.Length];
                        //MessageBox.Show("" + _fileEditor.GetDouble(_models[_models.Length - 1].Address));
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
