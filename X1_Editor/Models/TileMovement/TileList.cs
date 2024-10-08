using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X1_Editor.Models.Tiles
{
    public class TileList : IModelArray<Tile>
    {
        public TileList(IFileEditor fileEditor, ScenarioType scenario)
        {
            _fileEditor = fileEditor;
            Scenario = scenario;
        }

        public ScenarioType Scenario { get; }

        private Tile[] tilessorted;
        private Tile[] tiles;
        private IFileEditor _fileEditor;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            r = "Resources/MovementTypes.xml";

            tilessorted = new Tile[0];
            tiles = new Tile[31]; //max size of spellIndexList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Tile[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Tile[tilessorted.Length];
                        tilessorted.CopyTo(old, 0);
                        tilessorted = new Tile[old.Length + 1];
                        old.CopyTo(tilessorted, 0);
                        tilessorted[old.Length] = new Tile(_fileEditor, Scenario, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        tiles[tilessorted[old.Length].TileID] = tilessorted[old.Length];
                    }
                }
            }
            catch (FileLoadException)
            {
                return false;
                //} catch (FileNotFoundException) {
                //  return false;
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

        public Tile[] Models => tilessorted;
    }
}
