﻿using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;

namespace SF3.Models.X1.Tiles
{
    public class TileList : ModelArray<Tile>
    {
        public int MaxSize { get; } = 31;

        public TileList(IX1_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private Tile[] models;
        private IX1_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/MovementTypes.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new Tile[0];
            models = new Tile[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

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
                        old = new Tile[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new Tile[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new Tile(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[_models[_models.Length - 1].TileID] = _models[_models.Length - 1];
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
    }
}
