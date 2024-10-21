﻿using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;

namespace SF3.Models.X1.BattlePointers
{
    public class BattlePointersList : ModelArray<BattlePointers>
    {
        public int MaxSize { get; } = 5;

        public BattlePointersList(IX1_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private BattlePointers[] items;
        private IX1_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/BattlePointersList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new BattlePointers[0];
            items = new BattlePointers[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                BattlePointers[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new BattlePointers[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new BattlePointers[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new BattlePointers(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[_models[_models.Length - 1].BattleID] = _models[_models.Length - 1];
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
