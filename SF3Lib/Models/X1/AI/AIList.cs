using System;
using System.IO;
using SF3.Extensions;
using SF3.FileEditors;
using static SF3.Utils.Resources;

namespace SF3.Models.X1.AI {
    public class AIList : ModelArray<AI> {
        public int MaxSize { get; } = 130;

        public AIList(IX1_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;

            /*if (Globals.scenario == ScenarioType.Scenario1)
            {
                _resourceFile = "Resources/X1AIScn1.xml";
            }
            else if (Globals.scenario == ScenarioType.Scenario2)
            {
                _resourceFile = "Resources/X1AIOther.xml";
            }
            else if (Globals.scenario == ScenarioType.Scenario3)
            {
                _resourceFile = "Resources/X1AIOther.xml";
            }
            else if (Globals.scenario == ScenarioType.PremiumDisk)
            {
                _resourceFile = "Resources/X1AIOther.xml";
            }
            else
            {
                _resourceFile = "Resources/X1AIOther.xml";
            }*/
        }

        private IX1_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/X1AI.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _models = new AI[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open);
                var xml = MakeXmlReader(stream);
                xml.Read();
                while (!xml.EOF) {
                    xml.Read();
                    if (xml.HasAttributes) {
                        var newModel = new AI(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        _models = _models.ExpandedWith(newModel);
                        if (newModel.AIID < 0 || newModel.AIID >= MaxSize) {
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
