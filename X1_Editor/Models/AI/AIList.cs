﻿using System;
using System.Xml;
using System.IO;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.AI
{
    public static class AIList
    {
        private static AI[] spellssorted;
        private static AI[] spells;

        private static string r = "";

        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadAIList()
        {

            r = "Resources/X1AI.xml";

            /*if (Globals.scenario == 1)
            {
                r = "Resources/X1AIScn1.xml";
            }
            else if (Globals.scenario == 2)
            {
                r = "Resources/X1AIOther.xml";
            }
            else if (Globals.scenario == 3)
            {
                r = "Resources/X1AIOther.xml";
            }
            else if (Globals.scenario == 4)
            {
                r = "Resources/X1AIOther.xml";
            }
            else
            {
                r = "Resources/X1AIOther.xml";
            }*/


            spellssorted = new AI[0];
            spells = new AI[130]; //max size of spellList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                AI[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new AI[spellssorted.Length];
                        spellssorted.CopyTo(old, 0);
                        spellssorted = new AI[old.Length + 1];
                        old.CopyTo(spellssorted, 0);
                        spellssorted[old.Length] = new AI(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        spells[spellssorted[old.Length].AIID] = spellssorted[old.Length];
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

        public static AI[] getAIList()
        {
            return spellssorted;
        }
        public static AI getAI(int id)
        {
            return spells[id];
        }
    }
}
