using System;
using System.IO;
using SF3.Extensions;
using SF3.FileEditors;
using static SF3.Utils.Resources;

namespace SF3.Models.X013.StatusEffects {
    public class StatusEffectList : ModelArray<StatusEffect> {
        public int MaxSize { get; } = 1000;

        public StatusEffectList(IX013_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
        }

        private IX013_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/StatusGroupList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _models = new StatusEffect[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open);

                var xml = MakeXmlReader(stream);
                xml.Read();
                //int stop = 0;
                //int numberTest = 0;
                //while (!xml.EOF)
                int myCount = 0;
                //string myName = "WarpIndex " + myCount;
                //Globals.treasureDebug = true;
                //while (!xml.EOF && (_models.Length == 0 || newModel.Searched != 0xffff))

                while (!xml.EOF)
                //while (!xml.EOF && (_models.Length == 0 || (newModel.Searched != 0xffff || newModel.EventNumber != 0xffff)))
                //while (!xml.EOF && (_models.Length == 0 || myCount <= 2))
                {
                    {
                        xml.Read();
                        if (xml.HasAttributes) {
                            var newModel = new StatusEffect(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                            _models = _models.ExpandedWith(newModel);

                            if (newModel.StatusEffectID < 0 || newModel.StatusEffectID >= MaxSize) {
                                throw new IndexOutOfRangeException();
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
                if (stream != null) {
                    stream.Close();
                }
            }
            return true;
        }
    }
}
