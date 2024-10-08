using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X033_X031_Editor.Models.WeaponLevel
{
    public class WeaponLevelList : IModelArray<WeaponLevel>
    {
        ISF3FileEditor _fileEditor;

        public WeaponLevelList(ISF3FileEditor fileEditor)
        {
            _fileEditor = fileEditor;
        }

        public ScenarioType Scenario => _fileEditor.Scenario;

        private WeaponLevel[] itemssorted;
        private WeaponLevel[] items;

        private string r = "";

        /// <summary>
        /// Initialises list
        /// </summary>
        /// <returns>'true' on success, 'false' if .xml files do not exist or are in use</returns>
        public bool Load()
        {
            r = "Resources/WeaponLevel.xml";

            itemssorted = new WeaponLevel[0];
            items = new WeaponLevel[2]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

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
                        old = new WeaponLevel[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new WeaponLevel[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new WeaponLevel(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].WeaponLevelID] = itemssorted[old.Length];
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

        public WeaponLevel[] Models => itemssorted;
    }
}
