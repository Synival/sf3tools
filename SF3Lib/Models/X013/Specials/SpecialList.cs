using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;
using static SF3.Utils.Resources;
using SF3.Extensions;

namespace SF3.Models.X013.Specials
{
    public class SpecialList : ModelArray<Special>
    {
        public int MaxSize { get; } = 256;

        public SpecialList(IX013_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
            _resourceFile = ResourceFileForScenario(_fileEditor.Scenario, "Specials.xml");
        }

        private string _resourceFile;
        private IX013_FileEditor _fileEditor;
        private Special[] models;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new Special[0];
            models = new Special[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Special[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        _models = _models.ExpandedWith(new Special(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1)));
                        models[_models[_models.Length - 1].ID] = _models[_models.Length - 1];
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
