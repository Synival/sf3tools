using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;

namespace SF3.Models.X1.Headers
{
    public class HeaderList : ModelArray<Header>
    {
        public HeaderList(IX1_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private Header[] models;
        private IX1_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/X1Top.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new Header[0];
            models = new Header[31]; //max size of spellIndexList
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Header[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Header[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new Header[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new Header(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
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
