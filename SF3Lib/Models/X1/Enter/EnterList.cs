﻿using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;
using SF3.Extensions;
using static SF3.Utils.Resources;

namespace SF3.Models.X1.Enters
{
    public class EnterList : ModelArray<Enter>
    {
        public int MaxSize { get; } = 100;

        public EnterList(IX1_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX1_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/X1Enter.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new Enter[0];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                var xml = MakeXmlReader(stream);
                xml.Read();
                //int stop = 0;
                //int numberTest = 0;
                //while (!xml.EOF)
                int myCount = 0;
                //Globals.treasureDebug = true;
                //while (!xml.EOF && (_models.Length == 0 || _models[_models.Length - 1].Searched != 0xffff))

                /*if(Globals.treasureDebug == true)
                {
                    //while (!xml.EOF && (_models.Length == 0 || (_models[_models.Length - 1].Searched != 0xffff || _models[_models.Length - 1].EventNumber != 0xffff)))
                    while (!xml.EOF && (_models.Length == 0 || myCount <= 2))
                    {
                        {
                            xml.Read();
                            if (xml.HasAttributes)
                            {
                                var newModel = new Npc(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        _models = _models.ExpandedWith(newModel);
                                if (newModel.NpcID < 0 || newModel.NpcID >= MaxSize) {
throw new IndexOutOfRangeException();
}
                                if (newModel.SpriteID == 0xffff)
                                {
                                    myCount = 1 + myCount;
                                }
                            }
                        }
                    }
                }

                else*/
                {
                    while (!xml.EOF && (_models.Length == 0 || _models[_models.Length - 1].Entered != 0xffff))
                    //while (!xml.EOF && (_models.Length == 0 || (_models[_models.Length - 1].Searched != 0xffff || _models[_models.Length - 1].EventNumber != 0xffff)))
                    //while (!xml.EOF && (_models.Length == 0 || myCount <= 2))
                    {
                        {
                            xml.Read();
                            if (xml.HasAttributes)
                            {
                                var newModel = new Enter(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                                _models = _models.ExpandedWith(newModel);
                                if (newModel.EnterID < 0 || newModel.EnterID >= MaxSize)
                                {
                                    throw new IndexOutOfRangeException();
                                }
                                if (newModel.Entered == 0xffff)
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
    }
}
