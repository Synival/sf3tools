using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X1_Editor.Models.UnknownAI
{
    public class UnknownAIList : IModelArray<UnknownAI>
    {
        public UnknownAIList(ISF3FileEditor fileEditor)
        {
            _fileEditor = fileEditor;
        }

        public ScenarioType Scenario => _fileEditor.Scenario;

        private UnknownAI[] itemssorted;
        private UnknownAI[] items;
        private ISF3FileEditor _fileEditor;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            r = "Resources/UnknownAIList.xml";

            itemssorted = new UnknownAI[0];
            items = new UnknownAI[30]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                UnknownAI[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new UnknownAI[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new UnknownAI[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new UnknownAI(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].UnknownAIID] = itemssorted[old.Length];
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

        public UnknownAI[] Models => itemssorted;
    }
}
