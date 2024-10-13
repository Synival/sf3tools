using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X002_Editor.Models.Warps
{
    public class WarpList : ModelArray<Warp>
    {
        public WarpList(IX002_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX002_FileEditor _fileEditor;
        private Warp[] items;

        public override string ResourceFile => "Resources/X002Warp.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new Warp[0];
            items = new Warp[1000]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

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
                string myName = "WarpIndex " + myCount;
                //Globals.treasureDebug = true;
                //while (!xml.EOF && (_models.Length == 0 || _models[_models.Length - 1].Searched != 0xffff))

                while (!xml.EOF && (_models.Length == 0 || (_models[_models.Length - 1].WarpType != 0x01 && _models[_models.Length - 1].WarpType != 0xff)))
                //while (!xml.EOF && (_models.Length == 0 || (_models[_models.Length - 1].Searched != 0xffff || _models[_models.Length - 1].EventNumber != 0xffff)))
                //while (!xml.EOF && (_models.Length == 0 || myCount <= 2))
                {
                    {
                        xml.Read();
                        if (xml.HasAttributes)
                        {
                            old = new Warp[_models.Length];
                            _models.CopyTo(old, 0);
                            _models = new Warp[old.Length + 1];
                            old.CopyTo(_models, 0);
                            //_models[old.Length] = new Warp(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));

                            _models[old.Length] = new Warp(_fileEditor, myCount, myName);

                            myCount++;
                            myName = "WarpIndex ";
                            myName = myName + myCount;

                            items[_models[old.Length].WarpID] = _models[old.Length];
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
