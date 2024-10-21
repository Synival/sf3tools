﻿using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;

namespace SF3.Models.X013.Soulmate
{
    public class SoulmateList : ModelArray<Soulmate>
    {
        public int MaxSize { get; } = 1771;

        public SoulmateList(IX013_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX013_FileEditor _fileEditor;
        private Soulmate[] models;

        public override string ResourceFile => "Resources/SoulmateList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new Soulmate[0];
            models = new Soulmate[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Soulmate[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Soulmate[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new Soulmate[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new Soulmate(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[_models[_models.Length - 1].SoulmateID] = _models[_models.Length - 1];
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
