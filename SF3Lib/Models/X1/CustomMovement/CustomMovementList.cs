using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;
using SF3.Extensions;
using static SF3.Utils.Resources;

namespace SF3.Models.X1.CustomMovement
{
    public class CustomMovementList : ModelArray<CustomMovement>
    {
        public int MaxSize { get; } = 130;

        public CustomMovementList(IX1_FileEditor fileEditor) : base(fileEditor)
        {
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

        private CustomMovement[] models;
        private IX1_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/X1AI.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new CustomMovement[0];
            models = new CustomMovement[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);
                var xml = MakeXmlReader(stream);
                xml.Read();
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        _models = _models.ExpandedWith(new CustomMovement(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1)));
                        models[_models[_models.Length - 1].CustomMovementID] = _models[_models.Length - 1];
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
