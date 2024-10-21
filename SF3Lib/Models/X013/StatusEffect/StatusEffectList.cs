using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;
using SF3.Extensions;

namespace SF3.Models.X013.StatusEffects
{
    public class StatusEffectList : ModelArray<StatusEffect>
    {
        public int MaxSize { get; } = 1000;

        public StatusEffectList(IX013_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX013_FileEditor _fileEditor;
        private StatusEffect[] models;

        public override string ResourceFile => "Resources/StatusGroupList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new StatusEffect[0];
            models = new StatusEffect[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                StatusEffect[] old;
                //int stop = 0;
                //int numberTest = 0;
                //while (!xml.EOF)
                int myCount = 0;
                //string myName = "WarpIndex " + myCount;
                //Globals.treasureDebug = true;
                //while (!xml.EOF && (_models.Length == 0 || _models[_models.Length - 1].Searched != 0xffff))

                while (!xml.EOF)
                //while (!xml.EOF && (_models.Length == 0 || (_models[_models.Length - 1].Searched != 0xffff || _models[_models.Length - 1].EventNumber != 0xffff)))
                //while (!xml.EOF && (_models.Length == 0 || myCount <= 2))
                {
                    {
                        xml.Read();
                        if (xml.HasAttributes)
                        {
                            _models = _models.ExpandedWith(new StatusEffect(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1)));

                            models[_models[_models.Length - 1].StatusEffectID] = _models[_models.Length - 1];
                        }
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
