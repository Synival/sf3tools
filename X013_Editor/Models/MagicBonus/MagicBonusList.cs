using System;
using System.Xml;
using System.IO;
using static SF3.X013_Editor.Forms.frmMain;
using SF3.Models;
using SF3.Types;

namespace SF3.X013_Editor.Models.MagicBonus
{
    public class MagicBonusList : IModelArray<MagicBonus>
    {
        private MagicBonus[] itemssorted;
        private MagicBonus[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            if (Globals.scenario == ScenarioType.Scenario1)
            {
                r = "RSc1/magicBonusS1.xml";
            }
            else if (Globals.scenario == ScenarioType.Scenario2)
            {
                r = "RSc2/magicBonusS2.xml";
            }
            if (Globals.scenario == ScenarioType.Scenario3)
            {
                r = "Resources/magicBonus.xml";
            }
            else if (Globals.scenario == ScenarioType.PremiumDisk)
            {
                r = "RPD/magicBonusPD.xml";
            }

            itemssorted = new MagicBonus[0];
            items = new MagicBonus[256]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                MagicBonus[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new MagicBonus[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new MagicBonus[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new MagicBonus(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].MagicID] = itemssorted[old.Length];
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

        public MagicBonus[] Models => itemssorted;
    }
}
