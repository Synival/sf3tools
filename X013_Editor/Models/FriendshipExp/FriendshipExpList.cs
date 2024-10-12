using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X013_Editor.Models.Presets
{
    public class FriendshipExpList : IModelArray<FriendshipExp>
    {
        public FriendshipExpList(IX013_FileEditor fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX013_FileEditor _fileEditor;
        public ScenarioType Scenario => _fileEditor.Scenario;

        private FriendshipExp[] modelsSorted;
        private FriendshipExp[] models;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            r = "Resources/ExpList.xml";

            modelsSorted = new FriendshipExp[0];
            models = new FriendshipExp[1]; //max size of spellIndexList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                FriendshipExp[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new FriendshipExp[modelsSorted.Length];
                        modelsSorted.CopyTo(old, 0);
                        modelsSorted = new FriendshipExp[old.Length + 1];
                        old.CopyTo(modelsSorted, 0);
                        modelsSorted[old.Length] = new FriendshipExp(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[modelsSorted[old.Length].PresetID] = modelsSorted[old.Length];
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

        public FriendshipExp[] Models => modelsSorted;
    }
}
