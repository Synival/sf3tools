﻿using System;
using System.Xml;
using System.IO;
using static SF3.X1_Editor.Forms.frmMain;
using SF3.Models;

namespace SF3.X1_Editor.Models.CustomMovement
{
    public class CustomMovementList : IModelArray<CustomMovement>
    {
        private CustomMovement[] spellssorted;
        private CustomMovement[] spells;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            r = "Resources/X1AI.xml";

            /*
            if (Globals.scenario == 1)
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

            spellssorted = new CustomMovement[0];
            spells = new CustomMovement[130]; //max size of spellList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                CustomMovement[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new CustomMovement[spellssorted.Length];
                        spellssorted.CopyTo(old, 0);
                        spellssorted = new CustomMovement[old.Length + 1];
                        old.CopyTo(spellssorted, 0);
                        spellssorted[old.Length] = new CustomMovement(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        spells[spellssorted[old.Length].CustomMovementID] = spellssorted[old.Length];
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

        public CustomMovement[] Models => spellssorted;
    }
}
