using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;
using SF3.FileEditors;

namespace SF3.Models.X033_X031.WeaponLevel
{
    public class WeaponLevelList : ModelArray<WeaponLevel>
    {
        public WeaponLevelList(IX033_X031_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX033_X031_FileEditor _fileEditor;
        private WeaponLevel[] items;

        public override string ResourceFile => "Resources/WeaponLevel.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new WeaponLevel[0];
            items = new WeaponLevel[2]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                WeaponLevel[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new WeaponLevel[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new WeaponLevel[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new WeaponLevel(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[_models[old.Length].WeaponLevelID] = _models[old.Length];
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
