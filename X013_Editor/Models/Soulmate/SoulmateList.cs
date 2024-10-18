using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;
using SF3.X013_Editor.FileEditors;

namespace SF3.X013_Editor.Models.Soulmate
{
    public class SoulmateList : ModelArray<Soulmate>
    {
        public SoulmateList(IX013_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX013_FileEditor _fileEditor;
        private Soulmate[] items;

        public override string ResourceFile => "Resources/SoulmateList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new Soulmate[0];
            items = new Soulmate[1771]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Soulmate[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Soulmate[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new Soulmate[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new Soulmate(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[_models[old.Length].SoulmateID] = _models[old.Length];
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
