using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;
using static SF3.Utils.Resources;

namespace SF3.Models.IconPointerEditor.SpellIcons
{
    public class SpellIconList : ModelArray<SpellIcon>
    {
        public int MaxSize { get; } = 256;

        public SpellIconList(IIconPointerFileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
            _resourceFile = ResourceFileForScenario(_fileEditor.Scenario, "SpellIcons.xml");
        }

        private string _resourceFile;
        private SpellIcon[] models;
        private IIconPointerFileEditor _fileEditor;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new SpellIcon[0];
            models = new SpellIcon[MaxSize];
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
                        models[_models[_models.Length - 1].ID] = _models[_models.Length - 1];
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
