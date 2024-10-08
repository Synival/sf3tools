using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X013_Editor.Models.Critrate
{
    public class CritrateList : IModelArray<Critrate>
    {
        public CritrateList(IFileEditor fileEditor, ScenarioType scenario)
        {
            _fileEditor = fileEditor;
            Scenario = scenario;
        }

        private IFileEditor _fileEditor;
        public ScenarioType Scenario { get; }

        private Critrate[] itemssorted;
        private Critrate[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            r = "Resources/CritrateList.xml";

            itemssorted = new Critrate[0];
            items = new Critrate[3]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Critrate[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Critrate[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new Critrate[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new Critrate(_fileEditor, Scenario, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].CritrateID] = itemssorted[old.Length];
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

        public Critrate[] Models => itemssorted;
    }
}
