using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models.X1;
using static SF3.Utils.Resources;

namespace SF3.Tables.X1 {
    public class CustomMovementTable : Table<CustomMovement> {
        public int MaxSize { get; } = 130;

        public CustomMovementTable(IX1_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;

            /*
            if (Globals.scenario == ScenarioType.Scenario1)
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

        private readonly IX1_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/X1AI.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _rows = new CustomMovement[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);
                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                while (!xml.EOF) {
                    _ = xml.Read();
                    if (xml.HasAttributes) {
                        var newRow = new CustomMovement(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        _rows = _rows.ExpandedWith(newRow);
                        if (newRow.CustomMovementID < 0 || newRow.CustomMovementID >= MaxSize)
                            throw new IndexOutOfRangeException();
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