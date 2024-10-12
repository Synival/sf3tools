using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X1_Editor.Models.Slots
{
    public class SlotList : IModelArray<Slot>
    {
        public SlotList(IX1_FileEditor fileEditor)
        {
            _fileEditor = fileEditor;
        }

        public ScenarioType Scenario => _fileEditor.Scenario;

        private Slot[] modelsSorted;
        private Slot[] models;
        private IX1_FileEditor _fileEditor;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            if (Scenario == ScenarioType.Scenario1)
            {
                r = "Resources/X1List.xml";
            }
            else
                r = "Resources/X1OtherList.xml";

            modelsSorted = new Slot[0];
            models = new Slot[256]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Slot[] old;
                //int stop = 0;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Slot[modelsSorted.Length];
                        modelsSorted.CopyTo(old, 0);
                        modelsSorted = new Slot[old.Length + 1];
                        old.CopyTo(modelsSorted, 0);
                        modelsSorted[old.Length] = new Slot(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[modelsSorted[old.Length].ID] = modelsSorted[old.Length];
                        /*Console.WriteLine(items[itemssorted[old.Length].ID].EnemyID);
                        //numberTest = items[itemssorted[old.Length].ID].EnemyID;
                        if (items[itemssorted[old.Length].ID].EnemyID == 0xffff)
                        {
                            stop = 1;
                        }*/
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

        public Slot[] Models => modelsSorted;
    }
}
