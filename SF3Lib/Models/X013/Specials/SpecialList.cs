using System;
using System.IO;
using SF3.Extensions;
using SF3.FileEditors;
using static SF3.Utils.Resources;

namespace SF3.Models.X013.Specials {
    public class SpecialList : ModelArray<Special> {
        public int MaxSize { get; } = 256;

        public SpecialList(IX013_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
            _resourceFile = ResourceFileForScenario(_fileEditor.Scenario, "Specials.xml");
        }

        private string _resourceFile;
        private IX013_FileEditor _fileEditor;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _models = new Special[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                xml.Read();
                while (!xml.EOF) {
                    xml.Read();
                    if (xml.HasAttributes) {
                        var newModel = new Special(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        _models = _models.ExpandedWith(newModel);
                        if (newModel.ID < 0 || newModel.ID >= MaxSize) {
                            throw new IndexOutOfRangeException();
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
