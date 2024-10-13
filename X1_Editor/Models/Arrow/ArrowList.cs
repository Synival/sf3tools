using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X1_Editor.Models.Arrows
{
    public class ArrowList : ModelArray<Arrow>
    {
        private IX1_FileEditor _fileEditor;

        public ArrowList(IX1_FileEditor fileEditor)
        {
            _fileEditor = fileEditor;
        }

        public ScenarioType Scenario => _fileEditor.Scenario;

        private Arrow[] itemssorted;
        private Arrow[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public override bool Load()
        {
            r = "Resources/X1Arrow.xml";

            itemssorted = new Arrow[0];
            items = new Arrow[100]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Arrow[] old;
                //int stop = 0;
                //int numberTest = 0;
                //while (!xml.EOF)
                int myCount = 0;
                //Globals.treasureDebug = true;
                //while (!xml.EOF && (itemssorted.Length == 0 || itemssorted[itemssorted.Length - 1].Searched != 0xffff))

                /*if(Globals.treasureDebug == true)
                {
                    //while (!xml.EOF && (itemssorted.Length == 0 || (itemssorted[itemssorted.Length - 1].Searched != 0xffff || itemssorted[itemssorted.Length - 1].EventNumber != 0xffff)))
                    while (!xml.EOF && (itemssorted.Length == 0 || myCount <= 2))
                    {
                        {
                            xml.Read();
                            if (xml.HasAttributes)
                            {
                                old = new Npc[itemssorted.Length];
                                itemssorted.CopyTo(old, 0);
                                itemssorted = new Npc[old.Length + 1];
                                old.CopyTo(itemssorted, 0);
                                itemssorted[old.Length] = new Npc(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                                items[itemssorted[old.Length].NpcID] = itemssorted[old.Length];
                                if (itemssorted[itemssorted.Length - 1].SpriteID == 0xffff)
                                {
                                    myCount = 1 + myCount;
                                }
                            }
                        }
                    }
                }

                else*/
                {
                    while (!xml.EOF && (itemssorted.Length == 0 || itemssorted[itemssorted.Length - 1].ArrowUnknown0 != 0xffff))
                    //while (!xml.EOF && (itemssorted.Length == 0 || (itemssorted[itemssorted.Length - 1].Searched != 0xffff || itemssorted[itemssorted.Length - 1].EventNumber != 0xffff)))
                    //while (!xml.EOF && (itemssorted.Length == 0 || myCount <= 2))
                    {
                        {
                            xml.Read();
                            if (xml.HasAttributes)
                            {
                                old = new Arrow[itemssorted.Length];
                                itemssorted.CopyTo(old, 0);
                                itemssorted = new Arrow[old.Length + 1];
                                old.CopyTo(itemssorted, 0);
                                itemssorted[old.Length] = new Arrow(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                                items[itemssorted[old.Length].ArrowID] = itemssorted[old.Length];
                                if (itemssorted[itemssorted.Length - 1].ArrowUnknown0 == 0xffff)
                                {
                                    myCount = 1 + myCount;
                                }
                            }
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

        public Arrow[] Models => itemssorted;
    }
}
