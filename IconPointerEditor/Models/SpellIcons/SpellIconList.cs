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
        }

        private SpellIcon[] modelsSorted;
        private SpellIcon[] models;
        private IIconPointerFileEditor _fileEditor;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>'true' on success, 'false' if .xml files do not exist or are in use</returns>
        public override bool Load()
        {
            if (Scenario == ScenarioType.Scenario1)
            {
                r = "Resources/scenario1Spells.xml";
            }
            if (Scenario == ScenarioType.Scenario2)
            {
                r = "Resources/scenario2Spells.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                r = "Resources/scenario3Spells.xml";
            }
            if (Scenario == ScenarioType.PremiumDisk)
            {
                r = "Resources/PDSpells.xml";
            }

            modelsSorted = new SpellIcon[0];
            models = new SpellIcon[256]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

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
                        old = new SpellIcon[modelsSorted.Length];
                        modelsSorted.CopyTo(old, 0);
                        modelsSorted = new SpellIcon[old.Length + 1];
                        old.CopyTo(modelsSorted, 0);
                        modelsSorted[old.Length] = new SpellIcon(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[modelsSorted[old.Length].ID] = modelsSorted[old.Length];
                        //MessageBox.Show("" + _fileEditor.GetDouble(modelsSorted[modelsSorted.Length - 1].Address));
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

        public SpellIcon[] Models => modelsSorted;
    }
}
