using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;
using SF3.Extensions;

namespace SF3.Models.X1.Headers
{
    public class HeaderList : ModelArray<Header>
    {
        public int MaxSize { get; } = 31;

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
            models = new Header[MaxSize];
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
                        _models = _models.ExpandedWith(new Header(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1)));
                        models[_models[_models.Length - 1].SizeID] = _models[_models.Length - 1];
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
