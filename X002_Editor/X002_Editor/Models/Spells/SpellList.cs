using System;
//using System.Text;
using System.Xml;
using System.IO;
using static SF3.X002_Editor.Forms.frmMain;

namespace SF3.X002_Editor.Models.Spells
{
    public static class SpellList
    {
        private static Spell[] spellssorted;
        private static Spell[] spells;

        private static string r = "";

        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadSpellList()
        {

            if (Globals.scenario == 1)
            {
                r = "RSc1/spellListS1.xml";
            }
            else if (Globals.scenario == 2)
            {
                r = "RSc2/spellListS2.xml";
            }
            if (Globals.scenario == 3)
            {
                r = "Resources/spellList.xml";
            }
            else if (Globals.scenario == 4)
            {
                r = "RPD/spellListPD.xml";
            }


            spellssorted = new Spell[0];
            spells = new Spell[78]; //max size of spellList. 
            try {
                FileStream stream = new FileStream(r, FileMode.Open);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Spell[] old;
                while (!xml.EOF) {
                    xml.Read();
                    if (xml.HasAttributes) {
                        old = new Spell[spellssorted.Length];
                        spellssorted.CopyTo(old, 0);
                        spellssorted = new Spell[old.Length + 1];
                        old.CopyTo(spellssorted, 0);
                        spellssorted[old.Length] = new Spell(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        spells[spellssorted[old.Length].SpellID] = spellssorted[old.Length];
                    }
                }
                stream.Close();
            } catch (FileLoadException) {
                return false;
            } catch (FileNotFoundException) {
                return false;
            }
            return true;
        }

        public static Spell[] getSpellList()
        {
            return spellssorted;
        }
        public static Spell getSpell(int id)
        {
            return spells[id];
        }
    }
}
