﻿using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;
using SF3.Extensions;
using static SF3.Utils.Resources;

namespace SF3.Models.X013.SupportStats
{
    public class SupportStatsList : ModelArray<SupportStats>
    {
        public int MaxSize { get; } = 256;

        public SupportStatsList(IX013_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX013_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/X013StatList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new SupportStats[0];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                var xml = MakeXmlReader(stream);
                xml.Read();
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        var newModel = new SupportStats(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        _models = _models.ExpandedWith(newModel);
                        if (newModel.StatID < 0 || newModel.StatID >= MaxSize)
                        {
                            throw new IndexOutOfRangeException();
                        }
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
