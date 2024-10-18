using System;
using System.Xml;
using System.IO;
using SF3.Types;
using SF3.FileEditors;

namespace SF3.Models.IconPointerEditor.ItemIcons
{
    public class ItemIconList : ModelArray<ItemIcon>
    {
        public ItemIconList(IIconPointerFileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario1)
            {
                _resourceFile = "Resources/S1/Items.xml";
            }
            if (Scenario == ScenarioType.Scenario2)
            {
                _resourceFile = "Resources/S2/Items.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                _resourceFile = "Resources/S3/Items.xml";
            }
            if (Scenario == ScenarioType.PremiumDisk)
            {
                _resourceFile = "Resources/PD/Items.xml";
            }
        }

        private string _resourceFile;
        private ItemIcon[] models;
        private IIconPointerFileEditor _fileEditor;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new ItemIcon[0];
            models = new ItemIcon[300]; //max size 
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

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
