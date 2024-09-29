using System;
//using System.Text;
using System.Xml;
using System.IO;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.Tiles
{
    public static class TileList
    {
        private static Tile[] tilessorted;
        private static Tile[] tiles;

        private static string r = "";

        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadTileList()
        {

            
            
            r = "Resources/MovementTypes.xml";
            



            tilessorted = new Tile[0];
            tiles = new Tile[31]; //max size of spellIndexList
            try {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Tile[] old;
                while (!xml.EOF) {
                    xml.Read();
                    if (xml.HasAttributes) {
                        old = new Tile[tilessorted.Length];
                        tilessorted.CopyTo(old, 0);
                        tilessorted = new Tile[old.Length + 1];
                        old.CopyTo(tilessorted, 0);
                        tilessorted[old.Length] = new Tile(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        tiles[tilessorted[old.Length].TileID] = tilessorted[old.Length];
                    }
                }
                stream.Close();
            } catch (FileLoadException) {
                return false;
            //} catch (FileNotFoundException) {
              //  return false;
            }
            return true;
        }

        public static Tile[] getTileList()
        {
            return tilessorted;
        }
        public static Tile getTile(int id)
        {
            return tiles[id];
        }
    }
}
