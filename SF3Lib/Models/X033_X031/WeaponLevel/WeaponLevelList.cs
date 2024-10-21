using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;
using SF3.FileEditors;
using SF3.Extensions;

namespace SF3.Models.X033_X031.WeaponLevel
{
    public class WeaponLevelList : ModelArray<WeaponLevel>
    {
        public int MaxSize { get; } = 2;

        public WeaponLevelList(IX033_X031_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX033_X031_FileEditor _fileEditor;
        private WeaponLevel[] models;

        public override string ResourceFile => "Resources/WeaponLevel.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new WeaponLevel[0];
            models = new WeaponLevel[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        _models = _models.ExpandedWith(new WeaponLevel(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1)));
                        models[_models[_models.Length - 1].WeaponLevelID] = _models[_models.Length - 1];
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
