using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using static SF3.Utils.Resources;

namespace SF3.Models.X1.Treasure {
    public class TreasureList : ModelArray<Treasure> {
        public int MaxSize { get; } = 255;

        /// <summary>
        /// TODO: what does this do when set?
        /// </summary>
        public static bool Debug { get; set; } = false;

        public TreasureList(IX1_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
        }

        private readonly IX1_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/X1Treasure.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _models = new Treasure[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                //int stop = 0;
                //int numberTest = 0;
                //while (!xml.EOF)
                var myCount = 0;
                //Debug = true;
                //while (!xml.EOF && (_models.Length == 0 || newModel.Searched != 0xffff))

                if (Debug == true) {
                    //while (!xml.EOF && (_models.Length == 0 || (newModel.Searched != 0xffff || newModel.EventNumber != 0xffff)))
                    while (!xml.EOF && (_models.Length == 0 || myCount <= 2)) {
                        {
                            _ = xml.Read();
                            if (xml.HasAttributes) {
                                var newModel = new Treasure(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                                _models = _models.ExpandedWith(newModel);
                                if (newModel.TreasureID < 0 || newModel.TreasureID >= MaxSize)
                                    throw new IndexOutOfRangeException();
                                if (newModel.Searched == 0xffff)
                                    myCount = 1 + myCount;
                            }
                        }
                    }
                }
                else {
                    while (!xml.EOF && (_models.Length == 0 || _models[_models.Length - 1].Searched != 0xffff))
                    //while (!xml.EOF && (_models.Length == 0 || (_models[_models.Length - 1].Searched != 0xffff || _models[_models.Length - 1].EventNumber != 0xffff)))
                    //while (!xml.EOF && (_models.Length == 0 || myCount <= 2))
                    {
                        {
                            _ = xml.Read();
                            if (xml.HasAttributes) {
                                var newModel = new Treasure(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                                _models = _models.ExpandedWith(newModel);
                                if (newModel.TreasureID < 0 || newModel.TreasureID >= MaxSize)
                                    throw new IndexOutOfRangeException();
                                if (newModel.Searched == 0xffff)
                                    myCount = 1 + myCount;
                            }
                        }
                    }
                }
            }
            catch (FileLoadException) {
                return false;
            }
            catch (FileNotFoundException) {
                return false;
            }
            finally {
                stream?.Close();
            }
            return true;
        }
    }
}
