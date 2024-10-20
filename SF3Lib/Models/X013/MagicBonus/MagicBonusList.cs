using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;
using static SF3.Utils.Utils;

namespace SF3.Models.X013.MagicBonus
{
    public class MagicBonusList : ModelArray<MagicBonus>
    {
        public int MaxSize { get; } = 256;

        public MagicBonusList(IX013_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
            _resourceFile = ResourceFileForScenario(_fileEditor.Scenario, "MagicBonus.xml");
        }

        private string _resourceFile;
        private IX013_FileEditor _fileEditor;
        private MagicBonus[] items;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new MagicBonus[0];
            items = new MagicBonus[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                MagicBonus[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new MagicBonus[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new MagicBonus[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new MagicBonus(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[_models[old.Length].MagicID] = _models[old.Length];
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
