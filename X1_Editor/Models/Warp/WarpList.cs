﻿using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X1_Editor.Models.Warps
{
    public class WarpList : ModelArray<Warp>
    {
        public WarpList(IX1_FileEditor fileEditor)
        {
            _fileEditor = fileEditor;
        }

        public ScenarioType Scenario => _fileEditor.Scenario;

        private Warp[] itemssorted;
        private Warp[] items;
        private IX1_FileEditor _fileEditor;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public override bool Load()
        {
            r = "Resources/X1Warp.xml";

            itemssorted = new Warp[0];
            items = new Warp[255]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Warp[] old;
                //int stop = 0;
                //int numberTest = 0;
                //while (!xml.EOF)
                int myCount = 0;
                //Globals.treasureDebug = true;
                //while (!xml.EOF && (itemssorted.Length == 0 || itemssorted[itemssorted.Length - 1].Searched != 0xffff))

                while (!xml.EOF && (itemssorted.Length == 0 || (itemssorted[itemssorted.Length - 1].WarpType != 0x01 && itemssorted[itemssorted.Length - 1].WarpType != 0xff)))
                //while (!xml.EOF && (itemssorted.Length == 0 || (itemssorted[itemssorted.Length - 1].Searched != 0xffff || itemssorted[itemssorted.Length - 1].EventNumber != 0xffff)))
                //while (!xml.EOF && (itemssorted.Length == 0 || myCount <= 2))
                {
                    {
                        xml.Read();
                        if (xml.HasAttributes)
                        {
                            old = new Warp[itemssorted.Length];
                            itemssorted.CopyTo(old, 0);
                            itemssorted = new Warp[old.Length + 1];
                            old.CopyTo(itemssorted, 0);
                            itemssorted[old.Length] = new Warp(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                            items[itemssorted[old.Length].WarpID] = itemssorted[old.Length];
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

        public Warp[] Models => itemssorted;
    }
}
