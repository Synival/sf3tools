using System;
using System.Xml;
using System.IO;
using SF3.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using SF3.Types;

namespace SF3.X033_X031_Editor.Models.InitialInfos
{
    public class InitialInfoList : IModelArray<InitialInfo>
    {
        public InitialInfoList(ScenarioType scenario)
        {
            Scenario = scenario;
        }

        public ScenarioType Scenario { get; }

        private InitialInfo[] modelsSorted;
        private InitialInfo[] models;

        private string r = "";

        /// <summary>
        /// Initialises list
        /// </summary>
        /// <returns>'true' on success, 'false' if .xml files do not exist or are in use</returns>
        public bool Load()
        {
            if (Scenario == ScenarioType.Scenario1)
            {
                r = "RSc1/classEquipS1.xml";
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                r = "RSc2/classEquipS2.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                r = "Resources/classEquip.xml";
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                r = "RPD/classEquipPD.xml";
            }

            modelsSorted = new InitialInfo[0];
            models = new InitialInfo[100]; //max size of spellIndexList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                InitialInfo[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new InitialInfo[modelsSorted.Length];
                        modelsSorted.CopyTo(old, 0);
                        modelsSorted = new InitialInfo[old.Length + 1];
                        old.CopyTo(modelsSorted, 0);
                        modelsSorted[old.Length] = new InitialInfo(Scenario, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[modelsSorted[old.Length].PresetID] = modelsSorted[old.Length];
                    }
                }
                stream.Close();
            }
            catch (FileLoadException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            return true;
        }

        public InitialInfo[] Models => modelsSorted;
    }
}
